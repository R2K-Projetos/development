﻿//=========================
function TipoAbaPacienteControl(tipo) {

    if (tipo == 'p') {
        $('#btnSaveFormulario').show();
    }
    else {
        $('#btnSaveFormulario').hide();
    }
}