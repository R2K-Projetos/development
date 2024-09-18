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
