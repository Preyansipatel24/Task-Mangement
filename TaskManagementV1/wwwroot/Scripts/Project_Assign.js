
$(document).ready(function () {

    LoadDepartmentList();
    LoadDesignationList();
    GetProjectUserList();

    $("#btnCancel").click(function () {
        $.blockUI();
        RedirectToPage('/Project/Index');
    });
    $('.select2').select2();
});

function GetProjectUserList() {
    $.blockUI();
    ajaxCall("Get", false, '/ProjectAssignment/ProjectUserList?ProjectId=' + parseInt($("#hdnProjectId").val()) + '&DepartmentId=' + parseInt($("#ddlDepartment").val()) + '&DesignationId=' + parseInt($("#ddlDesignation").val()), null, function (result) {
        $("#divProjectAssignmentList").html(result.responseText);
        ApplyDatatable('tblProjectAssignmentList');
        $.unblockUI();

        $("#btnUpdateProjectAssignment").click(function () {
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Update it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.blockUI();
                    var reqData;
                    var UserIdList = [];

                    $(".chk-user-assignment").each(function () {
                        if ($(this).is(':checked')) {
                            var userId = parseInt($(this).attr('UserId'));
                            UserIdList.push(userId);
                        }
                    })

                    reqData = {
                        "ProjectId": parseInt($("#hdnProjectId").val()),
                        "UserIdList": UserIdList
                    }
                    ajaxCall("Post", false, '/ProjectAssignment/SaveUpdateProjectAssignment', JSON.stringify(reqData), function (result) {
                        if (result.status == true) {
                            Toast.fire({ icon: 'success', title: result.message });
                            RedirectToPage('/Project/Index');
                        }
                        else {
                            Toast.fire({ icon: 'error', title: result.message });
                            $.unblockUI();
                        }
                    });
                }
            });
        });
        $("#ddlDepartment").change(function () {
            GetProjectUserList();
        });
        $("#ddlDesignation").change(function () {
            GetProjectUserList();
        });
        
    });
}

function LoadDepartmentList() {
    $.blockUI();
    ajaxCall("Get", false, '/ProjectAssignment/GetDepartmentList?ProjectId=' + parseInt($("#hdnProjectId").val()), null, function (result) {
        $("#ddlDepartment").html('');
        $("#ddlDepartment").append('<option value="0"> All </option>');
        if (result.status == true) {
            $.each(result.data, function (index, value) {
                $("#ddlDepartment").append('<option  value="' + value.value + '">' + value.key + '</option>');
            });
        }
        $.unblockUI();
    });
}

function LoadDesignationList() {
    $.blockUI();
    ajaxCall("Get", false, '/ProjectAssignment/GetDesignationList?ProjectId=' + parseInt($("#hdnProjectId").val()), null, function (result) {
        $("#ddlDesignation").html('');
        $("#ddlDesignation").append('<option value="0"> All </option>');
        if (result.status == true) {
            $.each(result.data, function (index, value) {
                $("#ddlDesignation").append('<option  value="' + value.value + '">' + value.key + '</option>');
            });
        }
        $.unblockUI();
    });
}