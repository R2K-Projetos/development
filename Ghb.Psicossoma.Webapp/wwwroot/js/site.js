//======================
function OpenEdit(controller, view, Id) {

    let url = '/' + controller + '/' + view + '?id=' + Id;
    location.href = url;
}
