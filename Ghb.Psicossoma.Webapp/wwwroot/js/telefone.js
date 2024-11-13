//=========================
function MaskItem(o, f) {
    setTimeout(function () {
        var v = mphone(o.value);
        if (v != o.value) {
            o.value = v;
        }
    }, 1);
}
function mphone(v) {
    var r = v.replace(/\D/g, "");
    r = r.replace(/^0/, "");
    if (r.length > 10) {
        r = r.replace(/^(\d\d)(\d{5})(\d{4}).*/, "($1) $2-$3");
    } else if (r.length > 5) {
        r = r.replace(/^(\d\d)(\d{4})(\d{0,4}).*/, "($1) $2-$3");
    } else if (r.length > 2) {
        r = r.replace(/^(\d\d)(\d{0,5})/, "($1) $2");
    } else {
        r = r.replace(/^(\d*)/, "($1");
    }
    return r;
}
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
    $('#Telefone_DDDNum').val('');
    $("#TipoTelefoneId").val($("#TipoTelefoneId option:first").val());
}
//=========================
function AddTelefone() {

    const arrayDadosTefone = Array(3);
    let totalInserido = $('#hdnTotalTelefonesAdicionados').val();

    let numero = $('#Telefone_DDDNum').val();
    if (numero == '' || numero == undefined) {
        ShowAlert('danger', 'O campo <b>Número</b> deve ser informado.', 'alertaTelefone');
        return false;
    } 
    let codTipoTefone = RetornaComboSelecionado('TipoTelefoneId');
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
    arrayDadosTefone[2] = GetTextValueComboBox('TipoTelefoneId');
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
//=========================
function ListaTelefones() {

    $.ajax({
        type: 'GET',
        url: 'ObterPartialListaTelefones',
        data: { PessoaId: $('#PessoaId').val() },
        cache: false,
        async: true,
        success: function (retorno) {
            $('#listaTelefones').show();
            $('#listaTelefones').html(retorno);
            FormTelefoneControl(0);
        },
        failure: function (retorno) {
            alert('ajax.failure:\n'
                + 'data: ' + retorno + '\n'
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
//=========================
function EditTelefone(Id, PessoaId) {

    let tituloForm = 'Edita';
    let nomeBtn = 'Alterar';
    if (parseInt(Id) == 0) {
        tituloForm = 'Cadastra';
        nomeBtn = 'Cadastrar';
    }
    $.ajax({
        type: 'GET',
        url: 'ObterPartialFormTelefone',
        data: { Id: Id, PessoaId: PessoaId },
        cache: false,
        async: true,
        success: function (retorno) {
            FormTelefoneControl(1);
            $('#formTelefone').html(retorno);
            $("#tituloFormTelefone").html(tituloForm);
            $("#btnSalvaTelefone").html(nomeBtn);
        },
        failure: function (retorno) {
            alert('ajax.failure:\n'
                + 'data: ' + retorno + '\n'
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
//=========================
function CheckControl() {

    $('#Principal_S').is(":checked") ? Principal = true : Principal = false;
    $('#Ativo_S').is(":checked") ? Ativo = true : Ativo = false;
    if (Principal && !Ativo) {
        $('#Ativo_S').prop('checked', true);
    }
}
function SalvaTelefone(Id) {

    let PessoaId = $('#PessoaId').val();
    let TipoTelefoneId = $('#TipoTelefoneId').val();
    let Principal;
    $('#Principal_S').is(":checked") ? Principal = true : Principal = false;
    let DDDNum = $('#DDDNum').val();
    let Ativo;
    $('#Ativo_S').is(":checked") ? Ativo = true : Ativo = false;

    let model = {};
    model.Id = Id;
    model.PessoaId = PessoaId;
    model.TipoTelefoneId = TipoTelefoneId;
    model.Principal = Principal;
    model.DDDNum = DDDNum;
    model.Ativo = Ativo;

    var urlAjax = '/Telefone/';
    Id == 0 ? urlAjax += 'Create' : urlAjax += 'Edit';

    alert(''
        + 'model.Id: ' + model.Id + '\n'
        + 'model.PessoaId: ' + model.PessoaId + '\n'
        + 'model.TipoTelefoneId: ' + model.TipoTelefoneId + '\n'
        + 'model.Principal: ' + model.Principal + '\n'
        + 'model.DDDNum: ' + model.DDDNum + '\n'
        + 'model.Ativo: ' + model.Ativo + '\n'
        + 'urlAjax: ' + urlAjax + '\n'
    );
    //return false;    

    $.ajax({
        type: 'POST',
        url: urlAjax,
        data: model,
        dataType: "json",
        cache: false,
        async: true,
        success: function (retorno) {
            ListaTelefones();            
        },
        failure: function (retorno) {
            alert('ajax.failure:\n'
                + 'data: ' + retorno + '\n'
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
function DeleteTelefone(Id, PessoaId) {

    let model = {};
    model.Id = Id;
    model.PessoaId = PessoaId;
    model.TipoTelefoneId = 0;
    model.Principal = false;
    model.DDDNum = '';
    model.Ativo = false;

    var urlAjax = '/Telefone/Delete';

    alert(''
        + 'model.Id: ' + model.Id + '\n'
        + 'model.PessoaId: ' + model.PessoaId + '\n'
        + 'model.TipoTelefoneId: ' + model.TipoTelefoneId + '\n'
        + 'model.Principal: ' + model.Principal + '\n'
        + 'model.DDDNum: ' + model.DDDNum + '\n'
        + 'model.Ativo: ' + model.Ativo + '\n'
        + 'urlAjax: ' + urlAjax + '\n'
    );
    //return false;    

    $.ajax({
        type: 'POST',
        url: urlAjax,
        data: model,
        dataType: "json",
        cache: false,
        async: true,
        success: function (retorno) {
            ListaTelefones();
        },
        failure: function (retorno) {
            alert('ajax.failure:\n'
                + 'data: ' + retorno + '\n'
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