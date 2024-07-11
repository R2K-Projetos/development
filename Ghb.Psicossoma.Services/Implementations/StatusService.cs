using AutoMapper;
using System.Data;
using System.Diagnostics;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class StatusService : BaseService<StatusDto, Status>, IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IConfiguration _configuration;

        public StatusService(IStatusRepository statusRepository, IMapper mapper, IConfiguration configuration) : base(statusRepository, mapper)
        {
            _statusRepository = statusRepository;
            _configuration = configuration;
        }

        public override ResultDto<StatusDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            var returnValue = new ResultDto<StatusDto>();

            try
            {
                string selectQuery = $@"SELECT id, descricao FROM status;";

                DataTable result = _statusRepository.GetAll(selectQuery);

                if (result?.Rows.Count > 0)
                {
                    List<Status> statusList = [];
                    statusList = (from DataRow dr in result.Rows
                                  select new Status()
                                  {
                                      Id = Convert.ToInt32(dr["id"]),
                                      Descricao = dr["descricao"].ToString()
                                  }).ToList();

                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = statusList.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Status>, IEnumerable<StatusDto>>(statusList ?? Enumerable.Empty<Status>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, $"{_entityName.ToUpper()}_EMPTY" ?? "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

    }
}
