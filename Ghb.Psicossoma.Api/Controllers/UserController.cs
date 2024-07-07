using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Services.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }


    [HttpPost, Route("Authentication")]
    [SwaggerOperation(
    Summary = "Retorna um usuário específico de acordo com o seu identificador",
    Description = "Retorna um usuário específico de acordo com o seu identificador",
    OperationId = "User.Get",
    Tags = new[] { "User" })]
    public IActionResult Authentication(UserLoginDto login)
    {
        ResultDto<AuthenticationDto> result = new();

        try
        {
            result = _userService.Login(login.Email.ToLower(), login.Password);
        }
        catch (Exception ex)
        {
            result.BindError(500, ex.Message, ex);
        }

        return Ok(result);
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="userInfo">Json contendo os dados do novo usuário></param>
    /// <returns></returns>
    [HttpPost("Create")]
    [SwaggerOperation(
    Summary = "Cria um novo usuário",
    Description = "Cria um novo usuário",
    OperationId = "User.Create",
    Tags = new[] { "User" })
    ]
    public ActionResult<UserDto> Create([FromBody] UserDto userInfo)
    {
        ResultDto<UserDto> result = new();

        try
        {
            userInfo.Id = 0;
            userInfo.Senha = _userService.HashPasswordToString(userInfo.Senha);

            result = _userService.Insert(userInfo);

            if (!result.HasError)
                result.Message = "Usuário criado com sucesso!";
        }
        catch (Exception ex)
        {
            result.BindError(500, "Erro na criação do usuário", ex);
        }

        return Ok(result);
    }
}
