$(document).ready(function () {

    GetFilteredProductList("", "");
    applyRequiredValidation();

    $("#btnAddProject").click(function () {
        $.blockUI();
        RedirectToPage('/Project/AddEditProject?ProjectId=0');
    });

});

function GetFilteredProductList(searchByString, searchByStatus) {
    $.blockUI();
    ajaxCallDatatable("Get", false, "/Project/GetProjectList?searchByString=" + searchByString + "&searchByStatus=" + searchByStatus, null, function (result) {
        if (result.status == true) {
            console.log(result);
            var columns = [
                { title: 'Project Id', data: "id", "orderable": true, "searchable": true, class: "noVis" },
                { title: 'Project Name', data: "projectName", "orderable": true, "searchable": true },
                { title: 'Project Status', data: "projectStatus", "orderable": true, "searchable": true },
                { title: 'Start Date', data: "projectStartDateStr", "orderable": true, "searchable": true },
                { title: 'End Date', data: "projectEndDateStr", "orderable": true, "searchable": true },
                {
                    title: 'Actions', data: null, class: "text-center clsWrap noExport", "orderable": false, "searchable": false,
                    render: function (data, type, row, meta) {
                        return Actions(data.id);
                    }
                }
            ];

            ApplyDatatableWithData("tblProjectList", columns, result.data);
            $.unblockUI();
        }
        else {
            Toast.fire({ icon: 'error', title: result.message });
            $.unblockUI();
        }
        $('#tblProjectList').on('click', '.btn-edit', function () {
            $.blockUI();
            RedirectToPage('/Project/AddEditProject?ProjectId=' + $(this).attr('ProjectId'));
        });
        $('#tblProjectList').on('click', '.btn-asign', function () {
            $.blockUI();
            RedirectToPage('/ProjectAssignment/Index?ProjectId=' + $(this).attr('ProjectId'));
        });
    });
}

function Actions(id) {
    return '<i class="fa fa-pen text-warning btn-edit my-pointer" ProjectId="' + id + '" title="Edit" style="padding-right:10px;"> Edit</i> | <i class="fa fa-user-plus text-info btn-asign my-pointer" ProjectId="' + id + '" title="Edit" style="padding-left:10px;"> Asign</i>'
}
