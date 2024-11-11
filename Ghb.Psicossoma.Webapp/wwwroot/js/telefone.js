//=========================
function FormTelefoneControl(bVisualiza) {

    if (parseInt(bVisualiza) == 1) {
        $('#formTelefone').show();
        $('#btnNovoTelefone').hide();
    }
    else {
        $('#formTelefone').hide();
        $('#btnNovoTelefone').show();
    }
}
//=========================
function LimpaTelefone() {

    HideAlert('alertaTelefone');
    $('#txtNumeroTel').val('');
    $("#cmbTipoTelefone").val($("#cmbTipoTelefone option:first").val());
}
//=========================
function AddTelefone() {

    const arrayDadosTefone = Array(3);
    let totalInserido = $('#hdnTotalTelefonesAdicionados').val();

    let numero = $('#txtNumeroTel').val();
    if (numero == '' || numero == undefined) {
        ShowAlert('danger', 'O campo <b>Número</b> deve ser informado.', 'alertaTelefone');
        return false;
    } 
    let codTipoTefone = RetornaComboSelecionado('cmbTipoTelefone');
    if (parseInt(codTipoTefone) < 0) {
        ShowAlert('danger', 'O campo <b>Tipo</b> deve ser informado.', 'alertaTelefone');
        return false;
    }
    //let principal = 0;
    //if (totalInserido == 0) {
    //    principal = 1;
    //}
    let numeroItem = parseInt(totalInserido) + 1;

    arrayDadosTefone[0] = numeroItem;
    arrayDadosTefone[1] = numero;
    arrayDadosTefone[2] = GetTextValueComboBox('cmbTipoTelefone');
    arrayDadosTefone[3] = codTipoTefone;

    let novaLinha = MontaLinhaTelefone(arrayDadosTefone);

    $('#tableListaTelefone > tbody:last-child').append(novaLinha);
    $('#hdnTotalTelefonesAdicionados').val(numeroItem);

    LimpaTelefone();
    ControlaViewTableTelefone(true);
}
function RemoveTelefone(idLinha) {

    let totalInserido = $('#hdnTotalTelefonesAdicionados').val();
    let novoTotal = parseInt(totalInserido) - 1;
    $('#hdnTotalTelefonesAdicionados').val(novoTotal);
    $('#tableListaTelefone > tbody tr#' + idLinha).remove();

    if (parseInt(novoTotal) < 1) {
        ControlaViewTableTelefone(false);
    }
}
function MontaLinhaTelefone(arrayDados) {

    let montagem = '';
    montagem += '<tr id="' + arrayDados[0] + '">';
    montagem += '    <td>' + arrayDados[1] + '</td>';
    montagem += '    <td>' + arrayDados[2] + '</td>';
    montagem += '    <td class="text-center">';
    montagem += '        <div class="divIcons" onclick="RemoveTelefone(' + arrayDados[0] + ');">';
    montagem += '            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">';
    montagem += '                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />';
    montagem += '                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />';
    montagem += '            </svg>';
    montagem += '        </div>';
    montagem += '    </td>';
    montagem += '</tr>';

    return montagem;
}
function ControlaViewTableTelefone(bShowTable) {

    if (bShowTable) {
        $('#tableListaTelefone').show();
    }
    else {
        $('#tableListaTelefone').hide();
    }
}
function EditTelefone(Id) {

    $.ajax({
        type: 'GET',
        url: 'ObterPartialTelefone',
        data: { Id: Id },
        cache: false,
        async: true,
        success: function (data) {
            FormTelefoneControl(1);
            $('#formTelefone').html(data);
        },
        failure: function (data) {
            alert('ajax.failure:\n'
                + 'data: ' + data + '\n'
            );
        },
        error: function (req, status, error) {
            alert('ajax.error:\n'
                + 'req: ' + req + '\n'
                + 'status: ' + status + '\n'
                + 'error: ' + error + '\n'
            );
        }
    });
}