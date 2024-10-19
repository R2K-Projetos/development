$(document).ready($(function () {
    $('.dropdown-toggle').click(function () {
        location.href = this.href;
    });
}));
//======================
function CallAction(controller, action, Id) {

    let url = '/' + controller + '/' + action;
    if (Id != undefined)
        url += '?id=' + Id;

    location.href = url;
}
//======================
function OpenEdit(controller, view, Id) {

    let url = '/' + controller + '/' + view + '?id=' + Id;
    location.href = url;
}
//=======================================
//Retorna value comobox
// Ex: let valor = RetornaComboSelecionado('comboId');
//=======================================
function RetornaComboSelecionado(comboId) {
    return $('#' + comboId).find(":selected").val();
}
//=======================================
//Retorna value comobox
// Ex: let value = GetTextValueComboBox('comboNome');
//=======================================
function GetTextValueComboBox(sNomeCombo) {

    let e = document.getElementById(sNomeCombo);
    let itemSelecionado = e.options[e.selectedIndex].text;

    return itemSelecionado;
}
