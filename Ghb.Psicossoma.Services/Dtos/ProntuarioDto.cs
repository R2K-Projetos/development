﻿using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProntuarioDto :BaseDto
    {
        public int EncaminhamentoId { get; set; }
        public int ProfissionalId { get; set; }
        public int PacienteId { get; set; }
        public string DescricaoGeral { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
    }
}
