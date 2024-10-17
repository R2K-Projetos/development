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
    public class GrauParentescoController : BaseApiController
    {
        private readonly IGrauParentescoService _grauParentescoService;
        private readonly IConfiguration _configuration;

        public GrauParentescoController(IGrauParentescoService grauParentescoService, IConfiguration configuration)
        {
            _grauParentescoService = grauParentescoService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todas os Graus de parentescos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas os Graus de parentescos cadastrados",
        Description = "Lista todas os Graus de parentescos cadastrados",
        OperationId = "GrauParentesco.GetAll",
        Tags = new[] { "GrauParentesco" })]
        [ProducesResponseType(typeof(ResultDto<GrauParentescoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<GrauParentescoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<GrauParentescoDto> GetAll()
        {
            ResultDto<GrauParentescoDto> result = new();

            try
            {
                result = _grauParentescoService.GetAll();

                if (!result.HasError)
                    result.Message = "Graus de parentesco listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de Graus de parentesco", ex);
            }

            return Ok(result);
        }
    }
}
