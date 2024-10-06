using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.Services.Implementations;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CidadeController : BaseApiController
    {
        private readonly ICidadeService _cidadeService;
        private readonly IConfiguration _configuration;

        public CidadeController(ICidadeService cidadeService, IConfiguration configuration)
        {
            _cidadeService = cidadeService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado cid
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de uma determinada cidade",
        Description = "Busca os dados de uma determinada cidade",
        OperationId = "Cidade.Get",
        Tags = new[] { "Cidade" })]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidadeDto> Get(int id)
        {
            ResultDto<CidadeDto> result = new();

            try
            {
                result = _cidadeService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Cidade localizada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de Cidade", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todas as cidades cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas as cidades cadastradas",
        Description = "Lista todas as cidades cadastradas",
        OperationId = "Cidade.GetAll",
        Tags = new[] { "Cidade" })]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidadeDto> GetAll()
        {
            ResultDto<CidadeDto> result = new();

            try
            {
                result = _cidadeService.GetAll();

                if (!result.HasError)
                    result.Message = "Cidades listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de cidades", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca cidades de uma determinada UF
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllByUf")]
        [SwaggerOperation(
        Summary = "Busca cidades de uma determinada UF",
        Description = "Busca cidades de uma determinada UF",
        OperationId = "Cidade.GetAllByUf",
        Tags = new[] { "Cidade" })]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidadeDto> GetAllByUf(int ufId)
        {
            ResultDto<CidadeDto> result = new();

            try
            {
                result = _cidadeService.GetAllByUf(ufId);

                if (!result.HasError)
                    result.Message = "Cidades localizadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de cidades", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca cidades contendo nome ou parte do nome
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllByName")]
        [SwaggerOperation(
        Summary = "Busca cidades contendo nome ou parte do nome",
        Description = "Busca cidades contendo nome ou parte do nome",
        OperationId = "Cidade.GetAllByName",
        Tags = new[] { "Cidade" })]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<CidadeDto> GetAllByName(string name)
        {
            ResultDto<CidadeDto> result = new();

            try
            {
                result = _cidadeService.GetAllByName(name);

                if (!result.HasError)
                    result.Message = "Cidades localizadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de cidades", ex);
            }

            return Ok(result);
        }

    }
}
