﻿using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class StatusUsuarioDto : BaseDto
    {
        public string Descricao { get; set; } = string.Empty;
    }
}
