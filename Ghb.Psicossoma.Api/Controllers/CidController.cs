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
    public class CidController : BaseApiController
    {
        private readonly ICidService _cidService;
        private readonly IConfiguration _configuration;

        public CidController(ICidService especialidadeService, IConfiguration configuration)
        {
            _cidService = especialidadeService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado cid
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado cid",
        Description = "Busca os dados de um determinado cid",
        OperationId = "Cid.Get",
        Tags = new[] { "Cid" })]
        [ProducesResponseType(typeof(ResultDto<CidDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidDto> Get(int id)
        {
            ResultDto<CidDto> result = new();

            try
            {
                result = _cidService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Endereço localizada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de endereço", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca os dados de um determinado cid, pelo código
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByCode")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado cid, pelo código",
        Description = "Busca os dados de um determinado cid, pelo código",
        OperationId = "Cid.GetByCode",
        Tags = new[] { "Cid" })]
        [ProducesResponseType(typeof(ResultDto<CidDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidDto> GetByCode(string code)
        {
            ResultDto<CidDto> result = new();

            try
            {
                result = _cidService.GetByCode(code);

                if (!result.HasError)
                    result.Message = "Cid localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de cid", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os cids cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os cids cadastrados",
        Description = "Lista todos os cids cadastrados",
        OperationId = "Cid.GetAll",
        Tags = new[] { "Cid" })]
        [ProducesResponseType(typeof(ResultDto<CidDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidDto> GetAll()
        {
            ResultDto<CidDto> result = new();

            try
            {
                result = _cidService.GetAll();

                if (!result.HasError)
                    result.Message = "Cids listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de cids", ex);
            }

            return Ok(result);
        }

    }
}
