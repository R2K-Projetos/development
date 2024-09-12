function StartListaEntidade(entidade, bNovoCadastro) {
    $(document).ready(function () {

        let idInput = '#nome' + entidade;
        let idSpan = '';
        if (parseInt(bNovoCadastro) == 1) {
            idSpan = '#span' + entidade;
        }
        let idDataList = '#divDataList' + entidade;
        $(idInput).keyup(function () {

            let input = document.querySelector(idInput);
            let texto = input.value;
            alert(''
                + 'entidade: ' + entidade + '\n'
                + 'bNovoCadastro: ' + bNovoCadastro + '\n'
                + 'idInput: ' + idInput + '\n'
                + 'idSpan: ' + idSpan + '\n'
                + 'idDataList: ' + idDataList + '\n'
            );

            $.ajax({
                type: 'GET',
                url: '/' + entidade + '/GetByName',
                data: { name: texto },
                dataType: 'json',
                success: function (data) {
                    let id = '';
                    let nome = '';
                    let montagemDataList = '<datalist id="datalist' + entidade + '">';

                    var obj = data;
                    obj.forEach(function (item, index) {

                        id = $.trim(item.id);
                        nome = $.trim(item.nome);
                        montagemDataList += '<option data-value="' + id + '" value="' + nome + '">';
                    });
                    montagemDataList += '</datalist>';
                    alert(montagemDataList);

                    if (parseInt(bNovoCadastro) == 1) {
                        $(idSpan).hide();
                    }
                    $(idDataList).html(montagemDataList);
                    $(idDataList).show();
                },
                error: function (req, status, error) {
                    alert('error');
                }
            });
        });
    });
}
function SelectItemListEntidade(entidade, bNovoCadastro) {

    let idInput = '#nome' + entidade;
    let idSpan = '#span' + entidade;

    let idDataList = '#divDataList' + entidade;
    let idHdnCodigo = '#hdnCod' + entidade + 'Autocomplete';

    let textValue = $(idInput).val();
    let id = $(idDataList + ' [value="' + textValue + '"]').data('value');
    if (textValue != '' && id == undefined || id == '') {
        id = 0;
    }
    alert(''
        + 'entidade: ' + entidade + '\n'
        + 'idInput: ' + idInput + '\n'
        + 'idDataList: ' + idDataList + '\n'
        + 'idHdnCodigo: ' + idHdnCodigo + '\n'
        + 'id: ' + id + '\n'
    );
    $(idHdnCodigo).val(id);
    $(idDataList).hide();
    if (parseInt(bNovoCadastro) == 1) {
        if (id == 0) {
            $(idSpan).show();
            $(idSpan).html('<b>[Novo cadastro]</b>');
        }
    }
}
function SetValuesAutoComplete(entidade, nome, Id) {

    if (nome != '' && Id != '') {
        let idInput = '#nome' + entidade;
        let idHdnCodigo = '#hdnCod' + entidade + 'Autocomplete';

        //alert(''
        //    + 'entidade: ' + entidade + '\n'
        //    + 'nome: ' + nome + '\n'
        //    + 'Id: ' + Id + '\n'
        //    + 'idInput: ' + idInput + '\n'
        //    + 'idHdnCodigo: ' + idHdnCodigo + '\n'
        //);
        $(idInput).val(nome);
        $(idHdnCodigo).val(Id);
    }
}