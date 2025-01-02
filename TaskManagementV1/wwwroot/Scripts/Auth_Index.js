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