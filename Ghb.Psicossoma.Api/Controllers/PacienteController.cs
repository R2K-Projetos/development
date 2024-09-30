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
        /// Cria um novo Paciente
        /// </summary>
        /// <param name="obj">Json contendo os dados do novo paciente></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria um novo paciente",
        Description = "Cria um novo paciente",
        OperationId = "Paciente.Create",
        Tags = new[] { "Paciente" })]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PacienteResponseDto> Create([FromBody] PacienteResponseDto obj)
        {
            ResultDto<PacienteResponseDto> result = new();

            try
            {
                //obj.Id = 0;
                //result = _pacienteService.Insert(obj);

                //if (!result.HasError)
                    result.Message = "Paciente criado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação do Paciente", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de um Cid
        /// </summary>
        /// <param name="obj">Json contendo os dados da cid></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de um Paciente",
        Description = "Atualiza os dados de um Paciente",
        OperationId = "Paciente.Update",
        Tags = new[] { "Paciente" })]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PacienteResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PacienteResponseDto> Update([FromBody] PacienteResponseDto obj)
        {
            ResultDto<PacienteResponseDto> result = new();

            try
            {
                //result = _pacienteService.Update(obj);

                //if (!result.HasError)
                    result.Message = "Paciente alterado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração do Paciente", ex);
            }

            return Ok(result);
        }
    }
}
