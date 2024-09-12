$(document).ready($(function () {
    $('.dropdown-toggle').click(function () {
        location.href = this.href;
    });
}));
//======================
function OpenEdit(controller, view, Id) {

    let url = '/' + controller + '/' + view + '?id=' + Id;
    location.href = url;
}
