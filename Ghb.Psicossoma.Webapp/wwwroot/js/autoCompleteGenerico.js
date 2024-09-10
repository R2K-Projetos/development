function StartListaEntidade(sEntidade, bNovoCadastro) {
    $(document).ready(function () {

        let idInput = '#nome' + sEntidade;
        let idSpan = '';
        if (parseInt(bNovoCadastro) == 1) {
            idSpan = '#span' + sEntidade;
        }
        let idDataList = '#divDataList' + sEntidade;
        $(idInput).keyup(function () {

            let input = document.querySelector(idInput);
            let texto = input.value;
            //alert(''
            //    + 'idInput: ' + idInput + '\n'
            //    + 'idSpan: ' + idSpan + '\n'
            //    + 'idDataList: ' + idDataList + '\n'
            //);

            $.ajax({
                type: 'GET',
                url: '/' + sEntidade + '/GetByName',
                data: { name: texto },
                dataType: 'json',
                success: function (data) {
                    let id = '';
                    let nome = '';
                    let montagemDataList = '<datalist id="datalist' + sEntidade + '">';

                    var obj = data;
                    obj.forEach(function (item, index) {

                        id = $.trim(item.id);
                        nome = $.trim(item.nome);
                        montagemDataList += '<option data-value="' + id + '" value="' + nome + '">';
                    });
                    montagemDataList += '</datalist>';
                    //alert(montagemDataList);

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
function SelectItemListEntidade(sEntidade, bNovoCadastro) {

    let idInput = '#nome' + sEntidade;
    let idSpan = '#span' + sEntidade;

    let idDataList = '#divDataList' + sEntidade;
    let idHdnCodigo = '#hdnCod' + sEntidade + 'Autocomplete';

    let textValue = $(idInput).val();
    let id = $(idDataList + ' [value="' + textValue + '"]').data('value');
    if (textValue != '' && id == undefined || id == '') {
        id = 0;
    }
    //alert(''
    //    + 'sEntidade: ' + sEntidade + '\n'
    //    + 'idInput: ' + idInput + '\n'
    //    + 'idDataList: ' + idDataList + '\n'
    //    + 'idHdnCodigo: ' + idHdnCodigo + '\n'
    //    + 'id: ' + id + '\n'
    //);
    $(idHdnCodigo).val(id);
    $(idDataList).hide();
    if (parseInt(bNovoCadastro) == 1) {
        if (id == 0) {
            $(idSpan).show();
            $(idSpan).html('<b>[Novo cadastro]</b>');
        }
    }
}
function SetValuesAutoComplete(sEntidade, nomeTexto, Id) {

    if (nomeTexto != '' && Id != '') {
        let idInput = '#nome' + sEntidade;
        let idHdnCodigo = '#hdnCod' + sEntidade + 'Autocomplete';

        let textoFinal = nomeTexto;
        if (nomeTexto.indexOf('-#-') > 0) {
            textoFinal = nomeTexto.replace(/\-#-/g, '\'');
        }
        //alert(''
        //    + 'sEntidade: ' + sEntidade + '\n'
        //    + 'nomeTexto: ' + nomeTexto + '\n'
        //    + 'textoFinal: ' + textoFinal + '\n'
        //    + 'Id: ' + Id + '\n'
        //    + 'idInput: ' + idInput + '\n'
        //    + 'idHdnCodigo: ' + idHdnCodigo + '\n'
        //);
        $(idInput).val(textoFinal);
        $(idHdnCodigo).val(Id);
    }
}