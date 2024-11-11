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
        /// Lista todas as especialidades e marca as vinculadas ao profissional
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListaDisponivel/{ProfissionalId}")]
        [SwaggerOperation(
        Summary = "Lista todas as especialidades e marca as vinculadas ao profissional",
        Description = "Lista todas as especialidades e marca as vinculadas ao profissional",
        OperationId = "Especialidade.GetListaDisponivel",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EspecialidadeResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EspecialidadeResponseDto> GetListaDisponivel(int ProfissionalId)
        {
            ResultDto<EspecialidadeResponseDto> result = new();

            try
            {
                result = _especialidadeService.GetEspecialidadeDisponivel(ProfissionalId.ToString());

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
        /// Adiciona um Especialidade a um profissional
        /// <param name="obj">Json contendo os dados da classe profissionalespecialidade></param>
        /// </summary>
        /// <returns></returns>
        [HttpPost("AdicionaEspecialidade")]
        [SwaggerOperation(
        Summary = "Adiciona um Especialidade a um profissional",
        Description = "Adiciona um Especialidade a um profissional",
        OperationId = "Especialidade.AdicionaEspecialidade",
        Tags = new[] { "Especialidade" })]
        [ProducesResponseType(typeof(ResultDto<ProfissionalEspecialidadeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProfissionalEspecialidadeDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProfissionalEspecialidadeDto> AdicionaEspecialidade([FromBody] ProfissionalEspecialidadeDto obj)
        {
            ResultDto<ProfissionalEspecialidadeDto> result = new();

            try
            {
                result = _especialidadeService.AdicionaEspecialidade(obj);

                if (!result.HasError)
                    result.Message = "Especialidade adicionada com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na inclusão da Especialidade", ex);
            }

            return Ok(result);
        }
    }
}
