$(document).ready(function () {

    GetFilteredTaskList("", "", 0);
    applyRequiredValidation();

    $("#btnAddTask").click(function () {
        $.blockUI();
        RedirectToPage('/Task/AddEditTask?TaskId=0');
    });

});

function GetFilteredTaskList(searchByString, searchByStatus, ProjectId) {
    $.blockUI();
    ajaxCallDatatable("Get", false, "/Task/GetTaskList?searchByString=" + searchByString + "&searchByStatus=" + searchByStatus + "&projectId=" + ProjectId, null, function (result) {
        if (result.status == true) {
            console.log(result);
            var columns = [
                //{ title: 'Project Id', data: "id", "orderable": true, "searchable": true, class: "noVis" },
                { title: 'Project Name', data: "projectName", "orderable": true, "searchable": true },
                { title: 'Task Date', data: "taskDate", "orderable": true, "searchable": true },
                { title: 'Task Duration', data: "taskDuration", "orderable": true, "searchable": true },
                { title: 'Task Description', data: "taskDescription", "orderable": true, "searchable": true },
                { title: 'Task Status', data: "taskStatus", "orderable": true, "searchable": true },
                {
                    title: 'Actions', data: null, class: "text-center clsWrap noExport", "orderable": false, "searchable": false,
                    render: function (data, type, row, meta) {
                        return Actions(data.id);
                    }
                }
            ];

            ApplyDatatableWithData("tblTaskList", columns, result.data);
            $.unblockUI();
        }
        else {
            Toast.fire({ icon: 'error', title: result.message });
            $.unblockUI();
        }
        $('#tblTaskList').on('click', '.btn-edit', function () {
            $.blockUI();
            RedirectToPage('/Task/AddEditTask?TaskId=' + $(this).attr('TaskId'));
        });
    });
}

function Actions(id) {
    return '<i class="fa fa-pen text-warning btn-edit my-pointer" TaskId="' + id + '" title="Edit"> Edit</i>'
}
