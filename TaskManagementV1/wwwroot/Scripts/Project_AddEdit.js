$(document).ready(function () {
    LoadProjectStatusist();
    applyRequiredValidation();
    $("#btnCancel").click(function () {
        $.blockUI();
        RedirectToPage('/Project/Index');
    });

    $("#btnSaveUpdateProject").click(function () {
        if (validateRequiredFields()) {
            $.blockUI();
            var reqData = {
                "Id": parseInt($("#hdnProjectId").val()),
                "ProjectName": $("#txtProjectName").val(),
                "ProjectStatus": $("#ddlStatus").val(),
                "ProjectStartDate": $("#txtStartDate").val() != '' ? $("#txtStartDate").val() : null,
                "ProjectEndDate": $("#txtEndDate").val() != '' ? $("#txtEndDate").val() :null
            }
            ajaxCall("Post", false, '/Project/SaveUpdateProject', JSON.stringify(reqData), function (result) {
                if (result.status == true) {
                    Toast.fire({ icon: 'success', title: result.message });
                    RedirectToPage("/Project/Index");
                }
                else {
                    Toast.fire({ icon: 'error', title: result.message });
                    $.unblockUI();
                }
            });
        }
    });

});

function LoadProjectStatusist() {
    $.blockUI();
    ajaxCall("Get", false, '/Project/GetProjectStatusList', null, function (result) {
        $("#ddlStatus").html('');
        $("#ddlStatus").append('<option value=""> Select Status </option>');
        if (result.status == true) {
            $.each(result.data, function (index, value) {
                $("#ddlStatus").append('<option  value="' + value.value + '">' + value.label + '</option>');
            });
            if (parseInt($("#hdnProjectId").val()) > 0) {
                $("#ddlStatus").val($("#hdnStatus").val());
            }
        }
        $.unblockUI();
    });
}