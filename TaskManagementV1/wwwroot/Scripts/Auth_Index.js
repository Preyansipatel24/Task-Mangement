$(document).on('keypress', function (e) {
    if (e.which == 13) {
        $("#btnSignIn").click();
    }
});
$(document).ready(function () {
    applyRequiredValidation();
    $("#btnSignIn").click(function () {
        LoginV1();
    });

    $('#togglePassword').click(function () {
        var passwordField = $('#txtPassword');
        var passwordType = passwordField.attr('type');

        // Toggle the password visibility
        if (passwordType === 'password') {
            passwordField.attr('type', 'text');
            $(this).find('span').removeClass('fas fa-lock').addClass('fas fa-unlock'); // Change icon to eye
        } else {
            passwordField.attr('type', 'password');
            $(this).find('span').removeClass('fas fa-unlock').addClass('fas fa-lock'); // Change icon back to lock
        }
    });
});
function LoginV1() {
    if (validateRequiredFields()) {
        $.blockUI({ css: { backgroundColor: '#f00', color: '#fff' } })
        var loginReqModel = {
            "EmailId": $("#txtUserName").val(),
            "Password": $("#txtPassword").val()
        }
        $.ajax({
            type: "Post",
            url: "/Auth/Login",
            //contentType: 'application/json',
            data: loginReqModel,
            cache: false,
            success: function (result) {
                if (result.status == true) {
                    Toast.fire({ icon: 'success', title: result.message });
                    RedirectToPage("/Home/Dashboard");
                }
                else {
                    $.unblockUI();
                    Toast.fire({ icon: 'error', title: result.message });
                }
            },
            error: function (err) {
                $.unblockUI();
            }
        });
    }
}