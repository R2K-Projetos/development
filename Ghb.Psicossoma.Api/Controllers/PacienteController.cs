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
    public class PacienteController : BaseApiController
    {
        private readonly IPacienteService _pacienteService;
        private readonly IConfiguration _configuration;

        public PacienteController(IPacienteService pacienteService, IConfiguration configuration)
        {
            _pacienteService = pacienteService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado paciente
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado paciente",
        Description = "Busca os dados de um determinado paciente",
        OperationId = "Paciente.Get",
        Tags = new[] { "Paciente" })]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PacienteResponseDto> Get(int id)
        {
            ResultDto<PacienteResponseDto> result = new();

            try
            {
                result = _pacienteService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Paciente localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de paciente", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todas os pacientes cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas os pacientes cadastrados",
        Description = "Lista todas os pacientes cadastrados",
        OperationId = "Paciente.GetAll",
        Tags = new[] { "Paciente" })]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PacienteResponseDto> GetAll()
        {
            ResultDto<PacienteResponseDto> result = new();

            try
            {
                result = _pacienteService.GetAll();

                if (!result.HasError)
                    result.Message = "Pacientes listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de pacientes", ex);
            }

            return Ok(result);
        }

    }
}
