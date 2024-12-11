using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class StatusUsuarioController : BaseApiController
    {
        private readonly IStatusUsuarioService _statusService;
        private readonly IConfiguration _configuration;

        public StatusUsuarioController(IStatusUsuarioService statusService, IConfiguration configuration)
        {
            _statusService = statusService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos os status de acesso cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os status de acesso cadastrados",
        Description = "Lista todos os status de acesso cadastrados",
        OperationId = "StatusUsuario.GetAll",
        Tags = new[] { "StatusUsuario" })]
        [ProducesResponseType(typeof(ResultDto<StatusUsuarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<StatusUsuarioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<StatusUsuarioDto> GetAll()
        {
            ResultDto<StatusUsuarioDto> result = new();

            try
            {
                result = _statusService.GetAll();

                if (!result.HasError)
                    result.Message = "Status de acesso listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de status de acesso", ex);
            }

            return Ok(result);
        }

    }
}
