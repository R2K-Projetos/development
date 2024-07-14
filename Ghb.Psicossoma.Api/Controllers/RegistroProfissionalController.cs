using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Ghb.Psicossoma.Services.Dtos;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class RegistroProfissionalController : BaseApiController
    {
        private readonly IRegistroProfissionalService _registroProfissionalService;
        private readonly IConfiguration _configuration;

        public RegistroProfissionalController(IRegistroProfissionalService registroProfissionalService, IConfiguration configuration)
        {
            _registroProfissionalService = registroProfissionalService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos os registros profissionais cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os registros profissionais cadastrados",
        Description = "Lista todos os registros profissionais cadastrados",
        OperationId = "RegistroProfissional.GetAll",
        Tags = new[] { "RegistroProfissional" })]
        [ProducesResponseType(typeof(ResultDto<RegistroProfissionalDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<RegistroProfissionalDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<RegistroProfissionalDto> GetAll()
        {
            ResultDto<RegistroProfissionalDto> result = new();

            try
            {
                result = _registroProfissionalService.GetAll();

                if (!result.HasError)
                    result.Message = "Registros profissionais listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de registros profissionais", ex);
            }

            return Ok(result);
        }

    }
}
