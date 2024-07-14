using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class ProfissionalResponse : BaseEntity
    {
        public int PessoaId { get; set; }

        public string RegistroProfissional { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
    }
}
