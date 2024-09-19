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
    public class PlanoSaudeController : BaseApiController
    {
        private readonly IPlanoSaudeService _planoSaudeService;
        private readonly IConfiguration _configuration;

        public PlanoSaudeController(IPlanoSaudeService planoSaudeService, IConfiguration configuration)
        {
            _planoSaudeService = planoSaudeService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todas as pessoas cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas os Planos de Saúde cadastrados",
        Description = "Lista todas os Planos de Saúde cadastrados",
        OperationId = "PlanoSaude.GetAll",
        Tags = new[] { "PlanoSaude" })]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PlanoSaudeDto> GetAll()
        {
            ResultDto<PlanoSaudeDto> result = new();

            try
            {
                result = _planoSaudeService.GetAll();

                if (!result.HasError)
                    result.Message = "Planos de Saúde listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de Planos de Saúde", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca os dados de um determinado Plano de Saúde
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado Plano de Saúde",
        Description = "Busca os dados de um determinado Plano de Saúde",
        OperationId = "PlanoSaude.Get",
        Tags = new[] { "PlanoSaude" })]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PlanoSaudeDto> Get(int id)
        {
            ResultDto<PlanoSaudeDto> result = new();

            try
            {
                result = _planoSaudeService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Plano de Saúde localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de Plano de Saúde", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo cid
        /// </summary>
        /// <param name="obj">Json contendo os dados da nova cid></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria um novo Plano de saúde",
        Description = "Cria um novo Plano de saúde",
        OperationId = "PlanoSaude.Create",
        Tags = new[] { "PlanoSaude" })]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PlanoSaudeDto> Create([FromBody] PlanoSaudeDto obj)
        {
            ResultDto<PlanoSaudeDto> result = new();

            try
            {
                obj.Id = 0;
                result = _planoSaudeService.Insert(obj);

                if (!result.HasError)
                    result.Message = "Plano de saúde criado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação do Plano de saúde", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de um PlanoSaude
        /// </summary>
        /// <param name="obj">Json contendo os dados da cid></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de um Plano de saúde",
        Description = "Atualiza os dados de um Plano de saúde",
        OperationId = "PlanoSaude.Update",
        Tags = new[] { "PlanoSaude" })]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PlanoSaudeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PlanoSaudeDto> Update([FromBody] PlanoSaudeDto obj)
        {
            ResultDto<PlanoSaudeDto> result = new();

            try
            {
                result = _planoSaudeService.Update(obj);

                if (!result.HasError)
                    result.Message = "Plano de saúde alterado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração do Plano de saúde", ex);
            }

            return Ok(result);
        }
    }
}
