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
        /// Lista todos os cids cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas as funcionalidade cadastradas",
        Description = "Lista todas as funcionalidade cadastradas",
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
                    result.Message = "Funcionalidades listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de funcionalidades", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca os dados de uma determinada Funcionalidade
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de uma determinada Funcionalidade",
        Description = "Busca os dados de uma determinada Funcionalidade",
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
                    result.Message = "Funcionalidade localizada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização da FuncionalidadeDto", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova Funcionalidade
        /// </summary>
        /// <param name="obj">Json contendo os dados da nova Funcionalidade></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria uma nova Funcionalidade",
        Description = "Cria uma nova Funcionalidade",
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
                    result.Message = "Funcionalidade criada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação da Funcionalidade", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de uma funcionalidade
        /// </summary>
        /// <param name="obj">Json contendo os dados de uma funcionalidade></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de uma funcionalidade",
        Description = "Atualiza os dados de uma funcionalidade",
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
                    result.Message = "Funcionalidade alterada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração da Funcionalidade", ex);
            }

            return Ok(result);
        }
    }
}
