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

        /// <summary>
        /// Busca os dados de uma determinada especialidade 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de uma determinada especialidade",
        Description = "Busca os dados de uma determinada especialidade",
        OperationId = "Especialidade.Get",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeDto> Get(int id)
        {
            ResultDto<EspecialidadeDto> result = new();

            try
            {
                result = _especialidadeService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Especialidade localizada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização da Especialidade", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova especialidade
        /// </summary>
        /// <param name="obj">Json contendo os dados da nova cid></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria uma nova especialidade",
        Description = "Cria uma nova especialidade",
        OperationId = "Especialidade.Create",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeDto> Create([FromBody] EspecialidadeDto obj)
        {
            ResultDto<EspecialidadeDto> result = new();

            try
            {
                obj.Id = 0;
                result = _especialidadeService.Insert(obj);

                if (!result.HasError)
                    result.Message = "Especialidade criada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação da Especialidade", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de uma especialidade
        /// </summary>
        /// <param name="obj">Json contendo os dados de uma especialidade></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de uma especialidade",
        Description = "Atualiza os dados de uma especialidade",
        OperationId = "Especialidade.Update",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeDto> Update([FromBody] EspecialidadeDto obj)
        {
            ResultDto<EspecialidadeDto> result = new();

            try
            {
                result = _especialidadeService.Update(obj);

                if (!result.HasError)
                    result.Message = "Especialidade alterada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração da Especialidade", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todas as especialidades cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListaDisponivel/{ProfissionalId}")]
        [SwaggerOperation(
        Summary = "Lista todas as especialidades ainda não vinculadas a um profissional",
        Description = "Lista todas as especialidades ainda não vinculadas a um profissional",
        OperationId = "Especialidade.GetListaDisponivel",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeDto> GetListaDisponivel(int ProfissionalId)
        {
            ResultDto<EspecialidadeDto> result = new();

            try
            {
                result = _especialidadeService.GetEspecialidadeDisponivel(ProfissionalId);

                if (!result.HasError)
                    result.Message = "Especialidades listadas com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de especialidades", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todas as especialidades cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListaIndisponivel/{ProfissionalId}")]
        [SwaggerOperation(
        Summary = "Lista todas as especialidades vinculadas a um profissional",
        Description = "Lista todas as especialidades vinculadas a um profissional",
        OperationId = "Especialidade.GetListaIndisponivel",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeDto> GetListaIndisponivel(int ProfissionalId)
        {
            ResultDto<EspecialidadeDto> result = new();

            try
            {
                result = _especialidadeService.GetEspecialidadeIndisponivel(ProfissionalId);

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
