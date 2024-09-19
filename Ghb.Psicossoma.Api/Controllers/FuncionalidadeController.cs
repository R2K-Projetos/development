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
    public class FuncionalidadeController : BaseApiController
    {
        private readonly IFuncionalidadeService _funcionalidadeService;
        private readonly IConfiguration _configuration;

        public FuncionalidadeController(IFuncionalidadeService funcionalidadeService, IConfiguration configuration)
        {
            _funcionalidadeService = funcionalidadeService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todas as Funcionalidades cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas as Funcionalidades",
        Description = "Lista todas as Funcionalidades",
        OperationId = "Funcionalidade.GetAll",
        Tags = new[] { "Funcionalidade" })]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<FuncionalidadeDto> GetAll()
        {
            ResultDto<FuncionalidadeDto> result = new();

            try
            {
                result = _funcionalidadeService.GetAll();

                if (!result.HasError)
                    result.Message = "Funcionalidades listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de Funcionalidades", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca os dados de um determinado Plano de Saúde
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de uma determinada funcionalidade",
        Description = "Busca os dados de uma determinada funcionalidade",
        OperationId = "Funcionalidade.Get",
        Tags = new[] { "Funcionalidade" })]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<FuncionalidadeDto> Get(int id)
        {
            ResultDto<FuncionalidadeDto> result = new();

            try
            {
                result = _funcionalidadeService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Funcionalidade localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização da Funcionalidade", ex);
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
        Summary = "Cria uma nova funcionalidade",
        Description = "Cria uma nova funcionalidade",
        OperationId = "Funcionalidade.Create",
        Tags = new[] { "Funcionalidade" })]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<FuncionalidadeDto> Create([FromBody] FuncionalidadeDto obj)
        {
            ResultDto<FuncionalidadeDto> result = new();

            try
            {
                obj.Id = 0;
                result = _funcionalidadeService.Insert(obj);

                if (!result.HasError)
                    result.Message = "Funcionalidade criado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação da Funcionalidade", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de um Funcionalidade
        /// </summary>
        /// <param name="obj">Json contendo os dados da cid></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de uma Funcionalidade",
        Description = "Atualiza os dados de uma Funcionalidade",
        OperationId = "Funcionalidade.Update",
        Tags = new[] { "Funcionalidade" })]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<FuncionalidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<FuncionalidadeDto> Update([FromBody] FuncionalidadeDto obj)
        {
            ResultDto<FuncionalidadeDto> result = new();

            try
            {
                result = _funcionalidadeService.Update(obj);

                if (!result.HasError)
                    result.Message = "Funcionalidade alterado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração do Funcionalidade", ex);
            }

            return Ok(result);
        }
    }
}
