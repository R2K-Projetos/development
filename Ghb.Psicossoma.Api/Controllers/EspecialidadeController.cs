using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Swashbuckle.AspNetCore.Annotations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EspecialidadeController : BaseApiController
    {
        private readonly IEspecialidadeService _especialidadeService;
        private readonly IConfiguration _configuration;

        public EspecialidadeController(IEspecialidadeService especialidadeService, IConfiguration configuration)
        {
            _especialidadeService = especialidadeService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todas as especialidades cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todas as especialidades cadastradas",
        Description = "Lista todas as especialidades cadastradas",
        OperationId = "Especialidade.GetAll",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeDto> GetAll()
        {
            ResultDto<EspecialidadeDto> result = new();

            try
            {
                result = _especialidadeService.GetAll();

                if (!result.HasError)
                    result.Message = "Especialidades listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de especialidades", ex);
            }

            return Ok(result);
        }

    }
}
