using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.Services.Implementations;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : Controller
    {
        private readonly IPessoaService _pessoaService;
        private readonly IConfiguration _configuration;

        public PessoaController(IPessoaService pessoaService, IConfiguration configuration)
        {
            _pessoaService = pessoaService;
            _configuration = configuration;
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
        Tags = new[] { "Pessoa" })
        ]
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
