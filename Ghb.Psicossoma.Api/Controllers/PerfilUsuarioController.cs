using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PerfilUsuarioController : BaseApiController
    {
        private readonly IPerfilUsuarioService _perfilUsuarioService;
        private readonly IConfiguration _configuration;

        public PerfilUsuarioController(IPerfilUsuarioService perfilUsuarioService, IConfiguration configuration)
        {
            _perfilUsuarioService = perfilUsuarioService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos os perfis de acesso cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os perfis de acesso cadastrados",
        Description = "Lista todos os perfis de acesso cadastrados",
        OperationId = "PerfilUsuario.GetAll",
        Tags = new[] { "PerfilUsuario" })]
        [ProducesResponseType(typeof(ResultDto<PerfilUsuarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PerfilUsuarioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PerfilUsuarioDto> GetAll()
        {
            ResultDto<PerfilUsuarioDto> result = new();

            try
            {
                result = _perfilUsuarioService.GetAll();

                if (!result.HasError)
                    result.Message = "Perfis de acesso listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de perfis de acesso", ex);
            }

            return Ok(result);
        }

    }
}
