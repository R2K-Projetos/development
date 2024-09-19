//$.validator.setDefaults({
//    submitHandler: function () {
//alert("submitted!");
//return false;
//    }
//});

$(document).ready(function () {
    $("#formEntidade").validate({
        rules: {
            Nome: {
                required: true,
                minlength: 2,
                maxLength: 60
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