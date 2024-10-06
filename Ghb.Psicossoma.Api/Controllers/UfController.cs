using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Services.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Ghb.Psicossoma.Services.Implementations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UfController : BaseApiController
    {
        private readonly IUfService _ufService;
        private readonly IConfiguration _configuration;

        public UfController(IUfService ufService, IConfiguration configuration)
        {
            _ufService = ufService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todas as UFs cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas as UFs cadastradas",
        Description = "Lista todas as UFs cadastradas",
        OperationId = "Uf.GetAll",
        Tags = new[] { "Uf" })]
        [ProducesResponseType(typeof(ResultDto<UfDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<UfDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<UfDto> GetAll()
        {
            ResultDto<UfDto> result = new();

            try
            {
                result = _ufService.GetAll();

                if (!result.HasError)
                    result.Message = "UFs listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de UFs", ex);
            }

            return Ok(result);
        }
    }
}
