//=========================
$(document).ready(function () {
    $("#formProfissional").validate({
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
                required: false,
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
