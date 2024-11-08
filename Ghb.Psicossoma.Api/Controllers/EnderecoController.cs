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
    public class EnderecoController : BaseApiController
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IConfiguration _configuration;

        public EnderecoController(IEnderecoService enderecoService, IConfiguration configuration)
        {
            _enderecoService = enderecoService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado endereço
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado endereço",
        Description = "Busca os dados de um determinado endereço",
        OperationId = "Endereco.Get",
        Tags = new[] { "Endereco" })]
        [ProducesResponseType(typeof(ResultDto<EnderecoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EnderecoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EnderecoDto> Get(int id)
        {
            ResultDto<EnderecoDto> result = new();

            try
            {
                result = _enderecoService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Endereço localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de endereço", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os endereços cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os enderecos cadastrados",
        Description = "Lista todos os enderecos cadastrados",
        OperationId = "Endereco.GetAll",
        Tags = new[] { "Endereco" })]
        [ProducesResponseType(typeof(ResultDto<EnderecoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EnderecoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EnderecoDto> GetAll()
        {
            ResultDto<EnderecoDto> result = new();

            try
            {
                result = _enderecoService.GetAll();

                if (!result.HasError)
                    result.Message = "Endereços listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de endereços", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca os dados do endereço de uma pessoa
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEnderecoPessoa/{PessoaId}")]
        [SwaggerOperation(
        Summary = "Busca os dados do endereço de uma pessoa",
        Description = "Busca os dados do endereço de uma pessoa",
        OperationId = "Endereco.GetEnderecoPessoa",
        Tags = new[] { "Endereco" })]
        [ProducesResponseType(typeof(ResultDto<EnderecoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EnderecoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EnderecoDto> GetEnderecoPessoa(int PessoaId)
        {
            ResultDto<EnderecoDto> result = new();

            try
            {
                result = _enderecoService.GetEnderecoPessoa(PessoaId.ToString());

                if (!result.HasError)
                    result.Message = "Endereço localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de endereço", ex);
            }

            return Ok(result);
        }
    }
}
