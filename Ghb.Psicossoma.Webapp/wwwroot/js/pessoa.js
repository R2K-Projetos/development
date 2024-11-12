$(document).ready(function () {
    $('.cpf').mask('000.000.000-00');
});
//=========================
function TipoAbaControlPessoa(tipo) {

    if (tipo == 'p') {
        $('#btnSaveFormulario').show();
    }
    else {
        $('#btnSaveFormulario').hide();
    }
}