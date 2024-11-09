using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Services.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProfissionalController : BaseApiController
    {
        private readonly IProfissionalService _profissionalService;
        private readonly IConfiguration _configuration;

        public ProfissionalController(IProfissionalService profissionalService, IConfiguration configuration)
        {
            _profissionalService = profissionalService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado profissional
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado profissional",
        Description = "Busca os dados de um determinado profissional",
        OperationId = "Profissional.Get",
        Tags = new[] { "Profissional" })]
        [ProducesResponseType(typeof(ResultDto<ProfissionalResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProfissionalResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProfissionalResponseDto> Get(int id)
        {
            ResultDto<ProfissionalResponseDto> result = new();

            try
            {
                result = _profissionalService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Profissional localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de profissional", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todas os profissionais cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas os profissionais cadastrados",
        Description = "Lista todas os profissionais cadastrados",
        OperationId = "Profissional.GetAll",
        Tags = new[] { "Profissional" })]
        [ProducesResponseType(typeof(ResultDto<ProfissionalResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProfissionalResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProfissionalResponseDto> GetAll()
        {
            ResultDto<ProfissionalResponseDto> result = new();

            try
            {
                result = _profissionalService.GetAll();

                if (!result.HasError)
                    result.Message = "Profissionais listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de profissionais", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo profissional
        /// </summary>
        /// <param name="profissionalInfo">Json contendo os dados de um novo profissional></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria um novo profissional",
        Description = "Cria um novo profissional",
        OperationId = "Profissional.Create",
        Tags = new[] { "Profissional" })]
        [ProducesResponseType(typeof(ResultDto<ProfissionalDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProfissionalDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProfissionalDto> Create([FromBody] ProfissionalDto profissionalInfo)
        {
            ResultDto<ProfissionalDto> result = new();

            try
            {
                profissionalInfo.Id = 0;
                result = _profissionalService.Insert(profissionalInfo);

                if (!result.HasError)
                    result.Message = "Profissional criada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação de profissional", ex);
            }

            return Ok(result);
        }
    }
}
