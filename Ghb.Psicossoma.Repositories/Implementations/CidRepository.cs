using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class CidRepository : BaseRepository<Cid>, ICidRepository
    {
        public CidRepository(IContextDatabaseSettings settings) : base(settings)
        {

        }

        public DataTable GetByCode(string code)
        {
            MySqlConnection? cn = null;
            DataSet selectionData = new();
            string codeQuery = string.Empty;

            try
            {
                using (cn = new(_settings.ConnectionString))
                {
                    codeQuery = $@"SELECT Id, Codigo, Descricao
                                   FROM cid
                                   WHERE codigo = '{code}';";
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
