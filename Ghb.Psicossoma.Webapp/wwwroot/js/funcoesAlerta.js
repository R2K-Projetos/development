//================================
function ValidaCampo(objCampo, nomeCampo, idTarget) {

    if (!objCampo.value) {
        ShowAlert('danger', 'O campo <b>' + nomeCampo + '</b> deve ser informado.', idTarget);
        objCampo.focus();
        return false;
    }
    return true;
}
//============================================
// Alerta tootip
//============================================
function HideAlert(IdTarget) {
    if (IdTarget == '' || IdTarget == undefined)
        IdTarget = 'alertaPadrao';
    $("#" + IdTarget).hide();
}
function ShowAlert(sTipo, sMsg, IdTarget) {

    if (IdTarget == '' || IdTarget == undefined)
        IdTarget = 'alertaPadrao';

    $('#' + IdTarget).removeClass('alert-danger');
    $('#' + IdTarget).addClass('alert-' + sTipo);
    $('#' + IdTarget).hide();
    $('#' + IdTarget).find("span").html('');
    $('#' + IdTarget).show();
    $('#' + IdTarget).find("span").html(sMsg);
}
function ViewAlertRetorno(bStatusRetorno, IdTarget) {

    if (bStatusRetorno != '' && bStatusRetorno != undefined) {
        let sTipo = '';
        let sMsg = '';
        if (parseInt(bStatusRetorno) == 1) {
            sTipo = 'success';
            sMsg = '<b>Operação realizada com sucesso!</b>';
        }
        else {
            sTipo = 'danger';
            sMsg = '<b>Ocorreu um erro.</b>';
        }
        ShowAlert(sTipo, sMsg, IdTarget);
    }
}