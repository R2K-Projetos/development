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
    public class PessoaController : BaseApiController
    {
        private readonly IPessoaService _pessoaService;
        private readonly IConfiguration _configuration;

        public PessoaController(IPessoaService pessoaService, IConfiguration configuration)
        {
            _pessoaService = pessoaService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de uma determinada pessoa
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de uma determinada pessoa",
        Description = "Busca os dados de uma determinada pessoa",
        OperationId = "Pessoa.Get",
        Tags = new[] { "Pessoa" })]
        [ProducesResponseType(typeof(ResultDto<PessoaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PessoaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PessoaDto> Get(int id)
        {
            ResultDto<PessoaDto> result = new();

            try
            {
                result = _pessoaService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Pessoa localizada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de pessoa", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todas as pessoas cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas as pessoas cadastradas",
        Description = "Lista todas as pessoas cadastradas",
        OperationId = "Pessoa.GetAll",
        Tags = new[] { "Pessoa" })]
        [ProducesResponseType(typeof(ResultDto<PessoaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PessoaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PessoaDto> GetAll()
        {
            ResultDto<PessoaDto> result = new();

            try
            {
                result = _pessoaService.GetAll();

                if (!result.HasError)
                    result.Message = "Pessoas listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de pessoas", ex);
            }

            return Ok(result);
        }


        /// <summary>
        /// Cria uma nova pessoa
        /// </summary>
        /// <param name="pessoaInfo">Json contendo os dados da nova pessoa></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria uma nova pessoa",
        Description = "Cria uma nova pessoa",
        OperationId = "Pessoa.Create",
        Tags = new[] { "Pessoa" })]
        [ProducesResponseType(typeof(ResultDto<PessoaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PessoaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PessoaDto> Create([FromBody] PessoaDto pessoaInfo)
        {
            ResultDto<PessoaDto> result = new();

            try
            {
                pessoaInfo.Id = 0;
                result = _pessoaService.Insert(pessoaInfo);

                if (!result.HasError)
                    result.Message = "Pessoa criada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação de pessoa", ex);
            }

            return Ok(result);
        }
    }
}
