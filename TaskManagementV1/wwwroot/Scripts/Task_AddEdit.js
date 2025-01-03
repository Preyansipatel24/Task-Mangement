$(document).ready(function () {
    LoadProjectList();
    LoadTaskStatusist();
    applyRequiredValidation();
    $("#btnCancel").click(function () {
        $.blockUI();
        RedirectToPage('/Task/Index');
    });

    $("#btnSaveUpdateTask").click(function () {
        if (validateRequiredFields()) {
            $.blockUI();
            var reqData = {
                "Id": parseInt($("#hdnTaskId").val()),
                "ProjectId": parseInt($("#ddlProject").val()),
                "TaskDate": $("#txtTaskDate").val(),
                "TaskDuration": $("#txtTaskDuration").val(),
                "TaskDescription": $("#txtTaskDescription").val(),
                "TaskStatus": $("#ddlTaskStatus").val()
            }
            ajaxCall("Post", false, '/Task/SaveUpdateTask', JSON.stringify(reqData), function (result) {
                if (result.status == true) {
                    Toast.fire({ icon: 'success', title: result.message });
                    RedirectToPage("/Task/Index");
                }
                else {
                    Toast.fire({ icon: 'error', title: result.message });
                    $.unblockUI();
                }
            });
        }
    });

});

function LoadProjectList() {
    $.blockUI();
    ajaxCall("Get", false, '/Task/GetCurrentUserProjectList', null, function (result) {
        $("#ddlProject").html('');
        $("#ddlProject").append('<option value="0"> Select Project </option>');
        if (result.status == true) {
            $.each(result.data, function (index, value) {
                $("#ddlProject").append('<option  value="' + value.id + '">' + value.projectName + '</option>');
            });
            if (parseInt($("#hdnProjectId").val()) > 0) {
                $("#ddlProject").val($("#hdnProjectId").val());
            }
        }
        $.unblockUI();
    });
}

function LoadTaskStatusist() {
    $.blockUI();
    ajaxCall("Get", false, '/Task/GetTaskStatusList', null, function (result) {
        $("#ddlTaskStatus").html('');
        $("#ddlTaskStatus").append('<option value=""> Select Task Status </option>');
        if (result.status == true) {
            $.each(result.data, function (index, value) {
                $("#ddlTaskStatus").append('<option  value="' + value.value + '">' + value.label + '</option>');
            });
            if (parseInt($("#hdnProjectId").val()) > 0) {
                $("#ddlTaskStatus").val($("#hdnStatus").val());
            }
        }
        $.unblockUI();
    });
}