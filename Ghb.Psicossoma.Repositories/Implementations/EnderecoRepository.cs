using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class EnderecoRepository : BaseRepository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(IContextDatabaseSettings settings) : base(settings)
        {

        }

        public DataTable GetEnderecoPessoa(string PessoaId)
        {
            MySqlConnection? cn = null;
            DataSet selectionData = new();
            string codeQuery = string.Empty;

            try
            {
                using (cn = new(_settings.ConnectionString))
                {
                    codeQuery = $@"SELECT e.Id
                                          ,e.PessoaId
                                          ,e.CidadeId
                                          ,e.CEP
                                          ,e.Logradouro
                                          ,e.Numero
                                          ,e.Complemento
                                          ,e.Bairro
                                          ,e.Ativo
                                          ,c.UFId
                                     FROM endereco e
                                    INNER JOIN cidade c on c.Id = e.CidadeId
                                    WHERE e.PessoaId = {PessoaId};";
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
