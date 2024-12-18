using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class ConvenioRepository : BaseRepository<Convenio>, IConvenioRepository
    {
        public ConvenioRepository(IContextDatabaseSettings settings) : base(settings)
        {

        }

        public DataTable GetSomenteAtivos()
        {
            MySqlConnection? cn = null;
            DataSet selectionData = new();
            string codeQuery = string.Empty;

            try
            {
                using (cn = new(_settings.ConnectionString))
                {
                    codeQuery = $@"select Id
                                          ,Nome
                                     FROM convenio
                                    WHERE Ativo = 1
                                    order by Nome";
                    cn.Open();

                    using (MySqlCommand cmd = new(codeQuery, cn))
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
