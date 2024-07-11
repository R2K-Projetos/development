﻿using Microsoft.AspNetCore.Mvc;
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
    public class StatusController : BaseApiController
    {
        private readonly IStatusService _statusService;
        private readonly IConfiguration _configuration;

        public StatusController(IStatusService statusService, IConfiguration configuration)
        {
            _statusService = statusService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos os status de acesso cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os status de acesso cadastrados",
        Description = "Lista todos os status de acesso cadastrados",
        OperationId = "Status.GetAll",
        Tags = new[] { "Status" })]
        [ProducesResponseType(typeof(ResultDto<StatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<StatusDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<StatusDto> GetAll()
        {
            ResultDto<StatusDto> result = new();

            try
            {
                result = _statusService.GetAll();

                if (!result.HasError)
                    result.Message = "Status de acesso listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de status de acesso", ex);
            }

            return Ok(result);
        }

    }
}
