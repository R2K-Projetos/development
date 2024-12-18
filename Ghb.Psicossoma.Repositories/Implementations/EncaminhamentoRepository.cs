using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class EncaminhamentoRepository : BaseRepository<Encaminhamento>, IEncaminhamentoRepository
    {
        public EncaminhamentoRepository(IContextDatabaseSettings settings) : base(settings)
        {

        }
        public DataTable GetByIdPaciente(int id)
        {
            MySqlConnection? cn = null;
            DataSet selectionData = new();
            string selectQuery = string.Empty;

            try
            {
                using (cn = new(_settings.ConnectionString))
                {
                    selectQuery = $@"SELECT Id
                                            ,PacienteId
                                            ,EspecialidadeId
                                            ,ConvenioId
                                            ,CidId
                                            ,TotalSessoes
                                            ,MaximoSessoes
                                            ,SessoesRealizadas
                                            ,SolicitacaoMedica
                                            ,Observacao
                                            ,Ativo
                                       FROM encaminhamento
                                      WHERE Pacienteid = {id};";

                    cn.Open();

                    using (MySqlCommand cmd = new(selectQuery, cn))
                    {
                        MySqlDataAdapter store = new(cmd);
                        store.Fill(selectionData);
                    }

                    return selectionData.Tables[0];
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
