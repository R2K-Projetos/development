using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog.Context;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ghb.Psicossoma.Services.Implementations;

public class UserService : BaseService<UserDto, User>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;

    private readonly int keySize;
    private readonly int iterations;
    private readonly byte[] salt;
    private readonly HashAlgorithmName hashAlgorithm;

    public UserService(IUserRepository userRepository,
                       ILogger<UserService> logger,
                       IMapper mapper,
                       IConfiguration configuration) : base(userRepository, mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;

        keySize = 64;
        iterations = 350000;
        hashAlgorithm = HashAlgorithmName.SHA512;
        salt = new byte[] { 12, 75, 90, 120 };
    }

    public ResultDto<AuthenticationDto> Login(string email, string password)
    {
        ResultDto<AuthenticationDto> result = new();
        string filterQuery = $@"SELECT u.id, p.email, p.nome, u.senha
                                FROM pessoa p
                                INNER JOIN usuario u ON p.id = u.pessoaId
                                WHERE p.email = '{email}';";

        try
        {
            DataTable userResult = _userRepository.Get(filterQuery);

            if (userResult?.Rows.Count > 0)
            {
                DataRow? userRow = userResult.AsEnumerable().FirstOrDefault();

                if (VerifyPassword(password, userRow.Field<string>("senha")))
                {
                    (double tokenTtl, string token) = GenerateToken(userRow.Field<int>("id").ToString(), email, userRow.Field<string>("nome").ToString());

                    result.WasExecuted = true;
                    result.ResponseCode = 200;
                    result.Message = "Login efetuado com sucesso!";
                    result.Items = result.Items.Concat(
                        new AuthenticationDto[] {
                            new() {
                                Id = userRow.Field<int>("Id"),
                                Email = email,
                                Token = token,
                                TokenExpiration = DateTime.Now.AddHours(tokenTtl),
                            }
                        });

                    return result;
                }
            }

            result.BindError(404, "Usuário e/ou senha inválidos");
        }
        catch (Exception ex)
        {
            result.BindError(500, ex.GetErrorMessage());
            LogContext.PushProperty("Query", filterQuery);
            _logger.LogError(ex, "Erro na autenticação do usuário");
        }

        return result;
    }

    ResultDto<UserResponseDto> IUserService.Get(string id)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        ResultDto<UserResponseDto> returnValue = new();

        try
        {
            string selectQuery = $@"SELECT us.Id, ps.Nome, ps.Email, pfu.Descricao AS Perfil, st.Descricao AS Status, us.Ativo
                                    FROM usuario us
                                    INNER JOIN pessoa ps ON us.PessoaId = ps.Id
                                    INNER JOIN status st ON us.statusId = st.id
                                    INNER JOIN perfilUsuario pfu ON us.PerfilUsuarioId = pfu.Id
                                    WHERE us.Id = {id};";

            DataTable result = _userRepository.Get(selectQuery);
            List<UserResponse> users = result.CreateListFromTable<UserResponse>();

            if (users?.Count > 0)
            {
                returnValue.CurrentPage = 1;
                returnValue.PageSize = -1;
                returnValue.TotalItems = users.Count;
                returnValue.Items = _mapper.Map<IEnumerable<UserResponse>, IEnumerable<UserResponseDto>>(users ?? Enumerable.Empty<UserResponse>());
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            else
            {
                returnValue.BindError(404, "Não foram encontrados dados para exibição");
            }
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
            _logger.LogError(ex, "Erro na recuperação dos dados");
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    ResultDto<UserResponseDto> IUserService.GetAll()
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        ResultDto<UserResponseDto> returnValue = new();

        try
        {
            string selectQuery = $@"SELECT us.Id, ps.Nome, ps.Email, pfu.Descricao AS Perfil, st.Descricao AS Status, us.Ativo
                                    FROM usuario us
                                    INNER JOIN pessoa ps ON us.PessoaId = ps.Id
                                    INNER JOIN status st ON us.statusId = st.id
                                    INNER JOIN perfilUsuario pfu ON us.PerfilUsuarioId = pfu.Id;";

            DataTable result = _userRepository.GetAll(selectQuery);
            List<UserResponse> users = result.CreateListFromTable<UserResponse>();

            if (users?.Count > 0)
            {
                returnValue.CurrentPage = 1;
                returnValue.PageSize = -1;
                returnValue.TotalItems = users.Count;
                returnValue.Items = _mapper.Map<IEnumerable<UserResponse>, IEnumerable<UserResponseDto>>(users ?? Enumerable.Empty<UserResponse>());
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            else
            {
                returnValue.BindError(404, "Não foram encontrados dados para exibição");
            }
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
            _logger.LogError(ex, "Erro na recuperação dos dados");
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public override ResultDto<UserDto> Insert(UserDto dto)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<UserDto>();

        try
        {
            User? mapped = _mapper.Map<UserDto, User>(dto);
            string insertQuery = $@"INSERT INTO usuario(Id, PessoaId, PerfilUsuarioId, StatusId, Senha, Ativo) 
                                    VALUES(null, {mapped.PessoaId}, {mapped.PerfilUsuarioId}, {mapped.StatusId}, '{mapped.Senha}', {mapped.Ativo});";

            long newId =_userRepository.Insert(insertQuery);
            if (newId > 0)
                mapped.Id = (int)newId;

            UserDto? item = _mapper.Map<User, UserDto>(mapped);

            returnValue.Items = returnValue.Items.Concat(new[] { item });
            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
            _logger.LogError(ex, "Erro na gravação do dado");
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public override ResultDto<UserDto> Deactivate(string id)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        ResultDto<UserDto> returnValue = new();

        try
        {
            string updateQuery = $@"UPDATE usuario 
                                    SET Ativo = 0
                                    WHERE Id = {Convert.ToInt32(id)};";

            _userRepository.Update(updateQuery);
            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
            _logger.LogError(ex, "Erro na atualização do dado");
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public string HashPasswordToString(string password)
    {
        return Convert.ToHexString(HashPassword(password));
    }

    public byte[] HashPassword(string password)
    {
        return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                         salt,
                                         iterations,
                                         hashAlgorithm,
                                         keySize);
    }

    private (double, string) GenerateToken(string id, string email, string name)
    {
        string? authenticationTokenKey = _configuration.GetValue<string>("AuthenticationConfiguration:TokenKey");
        double authenticationTokenTtl = _configuration.GetValue<double>("AuthenticationConfiguration:TokenLoginTtl");

        if (authenticationTokenKey == null) throw new ArgumentNullException(nameof(authenticationTokenKey));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(authenticationTokenKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, id),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name)
            }),
            Expires = DateTime.UtcNow.AddHours(authenticationTokenTtl),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (authenticationTokenTtl, tokenHandler.WriteToken(token));
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return CryptographicOperations.FixedTimeEquals(HashPassword(password), Convert.FromHexString(hashedPassword));
    }
}
