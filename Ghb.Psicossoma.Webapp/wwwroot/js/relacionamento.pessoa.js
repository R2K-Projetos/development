function FormControleRelacionamento(tipo) {

    if (tipo == '') {
        $('#formRelacionamento').hide();
        $('#btnOpcoesRelaciomento').show();
    }
    else {
        $('#formRelacionamento').show();
        $('#btnOpcoesRelaciomento').hide();
        if (tipo == 'r') {
            $('#labelTipoRelacionamento').html('Responsável');
        }
        else {
            $('#labelTipoRelacionamento').html('Dependente');
        }
    }
}