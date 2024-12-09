﻿using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Pessoa : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;

        public string NomeReduzido { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
