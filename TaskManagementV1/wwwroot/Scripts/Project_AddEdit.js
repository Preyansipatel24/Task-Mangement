$(document).ready(function () {
    LoadProjectStatusList();
    LoadReportingPersonList();
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
                "ProjectEndDate": $("#txtEndDate").val() != '' ? $("#txtEndDate").val() : null,
                "ReportingPersonUserId": $("#ddlReportingPerson").val()
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
    $('.select2').select2();
});

function LoadProjectStatusList() {
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

function LoadReportingPersonList() {
    $.blockUI();
    ajaxCall("Get", false, '/Project/GetReportingPersonList', null, function (result) {
        $("#ddlReportingPerson").html('');
        $("#ddlReportingPerson").append('<option value="0"> Select Reporting Person </option>');
        if (result.status == true) {
            $.each(result.data, function (index, value) {
                $("#ddlReportingPerson").append('<option  value="' + value.userId + '">' + value.fullName + ' - ( ' + value.designationName + ' ) </option>');
            });
            if (parseInt($("#hdnddlReportingPerson").val()) > 0) {
                $("#ddlReportingPerson").val($("#hdnddlReportingPerson").val());
            }
        }
        $.unblockUI();
    });
}