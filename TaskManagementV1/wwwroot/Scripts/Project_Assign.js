
$(document).ready(function () {
    GetProjectUserList(parseInt($("#hdnProjectId").val()));

    $("#btnCancel").click(function () {
        $.blockUI();
        RedirectToPage('/Project/Index');
    });


});

function GetProjectUserList(ProjectId) {
    $.blockUI();
    ajaxCall("Get", false, '/ProjectAssignment/ProjectUserList?ProjectId=' + ProjectId, null, function (result) {
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
    });
}
