//=========================
function TipoAbaControlProfissional(tipo) {

    if (tipo == 'prin') {
        $('#btnSaveFormulario').show();
    }
    else {
        $('#btnSaveFormulario').hide();
        if (tipo == 'esp') {
            let ProfissionalId = $('#Id').val();
            ListaEspecialidades(ProfissionalId);
        }
    }    
}
//=========================
function ListaEspecialidades(ProfissionalId) {

    $.ajax({
        type: 'GET',
        url: 'ObterPartialEspecialidades',
        data: { ProfissionalId: ProfissionalId },
        cache: false,
        async: true,
        success: function (response) {
            $('#listaEpecialidades').html(response);
            $('#hdnProfissionalId').val(ProfissionalId);
        },
        failure: function (response) {
            alert('ajax.failure:\n'
                + 'response: ' + response + '\n'
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
function TrataVinculoEspecialidade(IdEspecialidade, Checked) {

    let sendType = 'POST';
    let Action = 'AdicionaEspecialidade';
    if (parseInt(Checked) == 1) {
        Action = 'RetiraEspecialidade';
        sendType = 'POST';
    }
    var urlAjax = '/Profissional/' + Action;

    let ProfissionalId = $('#Id').val();
    let ProfissionalId2 = $('#hdnProfissionalId').val();

    alert(''
        + 'ProfissionalId: ' + ProfissionalId + '\n'
        + 'ProfissionalId(2): ' + ProfissionalId2 + '\n'
    ); //return false;


    let model = {};
    model.Id = 0;
    model.ProfissionalId = ProfissionalId;
    model.EspecialidadeId = IdEspecialidade;
    model.Ativo = true;

    $.ajax({
        type: sendType,
        url: urlAjax,
        data: model,
        dataType: "json",
        success: function (response) {
            ListaEspecialidades(ProfissionalId);
            $('#nav-especialidade-tab').trigger('click');
        },
        failure: function (response) {
            alert('ajax.failure:\n'
                + 'response: ' + response + '\n'
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