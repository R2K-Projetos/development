$(document).ready(function () {
    $('.cep').mask('00000-000');
});
//==============================
function CarregaListaCidadesUF(Entidade, ufId) {

    let link = '/' + Entidade + '/FillCidadesUF?ufId=' + ufId;
    $(document).ready(function () {

        $('select#Endereco_CidadeId').empty();
        $.getJSON(link, function (data) {
            $('select#Endereco_CidadeId').append('<option value="0">[Selecione]</option>');
            $.each(data, function (i, item) {
                $('select#Endereco_CidadeId').append('<option value="' + item.id + '">' + item.nome + '</option>');
            });
        });
    });
}