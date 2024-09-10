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
    public class ProdutoConvenioController : BaseApiController
    {
        private readonly IProdutoConvenioService _produtoConvenioService;
        private readonly IConfiguration _configuration;

        public ProdutoConvenioController(IProdutoConvenioService produtoConvenioService,
                                         IConfiguration configuration)
        {
            _produtoConvenioService = produtoConvenioService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos os produtos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os produtos cadastrados",
        Description = "Lista todos os produtos cadastrados",
        OperationId = "ProdutoConvenio.GetAll",
        Tags = new[] { "ProdutoConvenio" })]
        [ProducesResponseType(typeof(ResultDto<ProdutoConvenioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProdutoConvenioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProdutoConvenioDto> GetAll()
        {
            ResultDto<ProdutoConvenioDto> result = new();

            try
            {
                result = _produtoConvenioService.GetAll();

                if (!result.HasError)
                    result.Message = "Produtos listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de produtos", ex);
            }

            return Ok(result);
        }


        /// <summary>
        /// Lista produtos cadastrados com determinada descrição
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByDescription")]
        [SwaggerOperation(
        Summary = "Lista produtos cadastrados com determinada descrição",
        Description = "Lista produtos cadastrados com determinada descrição",
        OperationId = "ProdutoConvenio.GetByDescription",
        Tags = new[] { "ProdutoConvenio" })]
        [ProducesResponseType(typeof(ResultDto<ProdutoConvenioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProdutoConvenioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProdutoConvenioDto> GetByDescription(string description)
        {
            ResultDto<ProdutoConvenioDto> result = new();

            try
            {
                result = _produtoConvenioService.GetByDescription(description);

                if (!result.HasError)
                    result.Message = "Produtos listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de produtos", ex);
            }

            return Ok(result);
        }
    }
}
