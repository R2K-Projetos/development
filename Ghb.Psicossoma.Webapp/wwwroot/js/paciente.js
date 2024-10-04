function TipoAbaPacienteControl(tipo) {

    if (tipo == 'p') {
        $('#btnSaveFormulario').show();
    }
    else {
        $('#btnSaveFormulario').hide();
    }
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
//=========================
function AddTelefone() {
    $(document).ready(function () {
        $("#formTelefone").validate({
            rules: {
                DDDNum: {
                    required: true,
                    minlength: 10,
                    maxLength: 20
                },
                Principal: {
                    required: true
                }
            },
            errorElement: "span",
            errorPlacement: function (error, element) {
                let customError = $([
                    '<span class="invalid-feedback mt-0 mb-2 d-block text-start">',
                    '  <span class="error-box mb-0 d-block"></span>',
                    '</span>'
                ].join(""));

                error.addClass("form-error-message");
                error.appendTo(customError.find('.error-box'));
                customError.insertBefore(element);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
                $(element).closest('.invalid-feedback').toggle();
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
                $(element).closest('.invalid-feedback').toggle();
            }
        });
        const arrayDadosTefone= Array(4);

        let totalInserido = $('#hdnTotalTelefonesAdicionados').val();
        let numero = $('#DDDNum').val();
        let codTipoTefone = RetornaComboSelecionado('cmbTipoTelefone');
        let principal = 0;        
        if (totalInserido == 0) {
            principal = 1;
        }
        let numeroItem = parseInt(totalInserido) + 1;

        arrayDadosTefone[0] = numeroItem;
        arrayDadosTefone[1] = numero;
        arrayDadosTefone[2] = GetTextValueComboBox('cmbTipoTelefone'); 
        arrayDadosTefone[3] = principal;
        arrayDadosTefone[4] = codTipoTefone; 

        let novaLinha = MontaLinhaTelefone(arrayDadosTefone);

        $('#tableListaTelefone > tbody:last-child').append(novaLinha);
        $('#hdnTotalTelefonesAdicionados').val(numeroItem);

        ControlaViewTableTelefone(true);
    });
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
    if (arrayDados[3] == 1) {
        montagem += '    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="green" class="bi bi-check2-circle" viewBox="0 0 16 16">';
        montagem += '        <path d="M2.5 8a5.5 5.5 0 0 1 8.25-4.764.5.5 0 0 0 .5-.866A6.5 6.5 0 1 0 14.5 8a.5.5 0 0 0-1 0 5.5 5.5 0 1 1-11 0" />';
        montagem += '        <path d="M15.354 3.354a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />';
        montagem += '    </svg>';
    }
    montagem += '    </td>';
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
$(document).ready(function () {
    $("#formPaciente").validate({
        rules: {
            Nome: {
                required: true,
                minlength: 2,
                maxLength: 100
            },
            NomeReduzido: {
                required: true,
                minlength: 2,
                maxLength: 40
            },
            Cpf: {
                required: true,
                minlength: 14,
                maxLength: 14
            },
            Email: {
                required: true,
                email: true,
                maxLength: 80
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            let customError = $([
                '<span class="invalid-feedback mt-0 mb-2 d-block text-start">',
                '  <span class="error-box mb-0 d-block"></span>',
                '</span>'
            ].join(""));

            error.addClass("form-error-message");
            error.appendTo(customError.find('.error-box'));
            customError.insertBefore(element);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
            $(element).closest('.invalid-feedback').toggle();
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
            $(element).closest('.invalid-feedback').toggle();
        }
    });
});
