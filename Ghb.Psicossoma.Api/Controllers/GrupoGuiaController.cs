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
    public class GrupoGuiaController : BaseApiController
    {
        private readonly IGrupoGuiaService _grupoGuiaService;
        private readonly IConfiguration _configuration;

        public GrupoGuiaController(IGrupoGuiaService grupoGuiaService, IConfiguration configuration)
        {
            _grupoGuiaService = grupoGuiaService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado grupo de guias
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado grupo de guias",
        Description = "Busca os dados de um determinado grupo de guias",
        OperationId = "GrupoGuia.Get",
        Tags = new[] { "GrupoGuia" })]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<GrupoGuiaDto> Get(int id)
        {
            ResultDto<GrupoGuiaDto> result = new();

            try
            {
                result = _grupoGuiaService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Grupo de guias localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de grupo de guias", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os grupos de guias cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os grupos de guias cadastrados",
        Description = "Lista todos os grupos de guias cadastrados",
        OperationId = "GrupoGuia.GetAll",
        Tags = new[] { "GrupoGuia" })]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<GrupoGuiaDto> GetAll()
        {
            ResultDto<GrupoGuiaDto> result = new();

            try
            {
                result = _grupoGuiaService.GetAll();

                if (!result.HasError)
                    result.Message = "Grupos de guias listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de grupos de guias", ex);
            }

            return Ok(result);
        }


        /// <summary>
        /// Cria um novo grupo de guias
        /// </summary>
        /// <param name="grupoGuia">Json contendo os dados do novo grupo de guias></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria um novo grupo de guias",
        Description = "Cria um novo grupo de guias",
        OperationId = "GrupoGuia.Create",
        Tags = new[] { "GrupoGuia" })]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<GrupoGuiaDto> Create([FromBody] GrupoGuiaDto grupoGuia)
        {
            ResultDto<GrupoGuiaDto> result = new();

            try
            {
                grupoGuia.Id = 0;
                result = _grupoGuiaService.Insert(grupoGuia);

                if (!result.HasError)
                    result.Message = "Grupo de guias criado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação de grupo de guias", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de um grupo de guias
        /// </summary>
        /// <param name="grupoGuia">Json contendo os dados do grupo de guias></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de um grupo de guias",
        Description = "Atualiza os dados de um grupo de guias",
        OperationId = "GrupoGuia.Update",
        Tags = new[] { "GrupoGuia" })]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<GrupoGuiaDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<GrupoGuiaDto> Update([FromBody] GrupoGuiaDto grupoGuia)
        {
            ResultDto<GrupoGuiaDto> result = new();

            try
            {
                result = _grupoGuiaService.Update(grupoGuia);

                if (!result.HasError)
                    result.Message = "Grupo de guias alterado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração do grupo de guias", ex);
            }

            return Ok(result);
        }
    }
}
