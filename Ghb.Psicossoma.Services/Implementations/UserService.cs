using AutoMapper;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using Ghb.Psicossoma.Services.Dtos;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Ghb.Psicossoma.Domains.Entities;
using System.IdentityModel.Tokens.Jwt;
using Ghb.Psicossoma.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Implementations;

public class UserService : BaseService<UserDto, User>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    private readonly int keySize;
    private readonly int iterations;
    private readonly byte[] salt;
    private readonly HashAlgorithmName hashAlgorithm;

    public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration) : base(userRepository, mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;

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

        return result;
    }

    public override ResultDto<UserDto> Insert(UserDto dto)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<UserDto>();

        try
        {
            var f = _mapper.Map<UserDto, User>(dto);
            string insertQuery = $@"INSERT INTO usuario(id, pessoaId, perfilUsuarioId, statusId, senha, ativo) 
                                    VALUES(null, {f.PessoaId}, {f.PerfilUsuarioId}, {f.StatusId}, '{f.Senha}', {f.Ativo});";

            long newId =_userRepository.Insert(insertQuery);
            if (newId > 0)
            {
                f.Id = (int)newId;
            }

            var item = _mapper.Map<User, UserDto>(f);

            returnValue.Items = returnValue.Items.Concat(new[] { item });
            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
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
