function CarregaListaCidadesUF(Entidade, ufId) {
    $(document).ready(function () {

        $('select#cmbCidade').empty();

        $.getJSON('/' + Entidade + '/FillCidadesUF?ufId=' + ufId, function (data) {
            $('select#cmbCidade').append('<option value="0">[Selecione]</option>');
            $.each(data, function (i, item) {
                $('select#cmbCidade').append('<option value="' + item.id + '">' + item.nome + '</option>');
            });
        });
    });
}