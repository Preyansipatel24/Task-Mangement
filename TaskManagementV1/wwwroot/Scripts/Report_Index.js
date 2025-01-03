$(document).ready(function () {

    $('#Project-Wise-Report-tab').on('click', function () {
        $('#custom-report-tab a[href="#Project-Wise-Report"]').tab('show');
        GetFilteredProductList("", "");
    });

    $('#User-Wise-Report-tab').on('click', function () {
        $('#custom-report-tab a[href="#User-Wise-Report"]').tab('show');
        GetUserList();
    });

    $('#Project-Wise-Report-tab').click();

});

function GetFilteredProductList(searchByString, searchByStatus) {
    $.blockUI();
    ajaxCallDatatable("Get", false, "/Report/GetProjectList?searchByString=" + searchByString + "&searchByStatus=" + searchByStatus, null, function (result) {
        if (result.status == true) {
            console.log(result);
            var columns = [
                //{ title: 'Project Id', data: "id", "orderable": true, "searchable": true, class: "noVis" },
                { title: 'Project Name', data: "projectName", "orderable": true, "searchable": true },
                { title: 'Project Status', data: "projectStatus", "orderable": true, "searchable": true },
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
        $('#tblProjectList').on('click', '.btn-download', function () {
            $.blockUI();
            ajaxCall("Get", false, '/Report/DownloadReport?IsUserWiseReport=false&UserId=0&ProjectId=' + $(this).attr('ProjectId') + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val(), null, function (result) {
                if (result.status == true) {
                    // If the result is successful and contains the file path
                    var filePath = result.data; // Assuming result contains a field 'filePath'

                    // Open the file in a new tab
                    var newTab = window.open(filePath, '_blank');

                    // Optional: Check if the tab was successfully opened
                    if (newTab) {
                        newTab.focus(); // Brings the new tab into focus
                    } else {
                        Toast.fire({ icon: 'error', title: 'Unable to open the file. Please check your browser settings.' });
                    }
                }
                else {
                    Toast.fire({ icon: 'error', title: 'Error while downloading the file' });
                }
                $.unblockUI();
            });
            //RedirectToPage('/Report/DownloadReport?IsUserWiseReport=false&UserId=0&ProjectId=' + $(this).attr('ProjectId'));
        });
       
    });
}

function GetUserList() {
    $.blockUI();
    ajaxCallDatatable("Get", false, "/Report/GetUserList", null, function (result) {
        if (result.status == true) {
            console.log(result);
            var columns = [
                //{ title: 'Project Id', data: "id", "orderable": true, "searchable": true, class: "noVis" },
                { title: 'Full Name', data: "fullName", "orderable": true, "searchable": true },
                { title: 'Email', data: "email", "orderable": true, "searchable": true },
                { title: 'Role', data: "roleName", "orderable": true, "searchable": true },
                { title: 'Designation', data: "designationName", "orderable": true, "searchable": true },
                { title: 'Department', data: "departmentName", "orderable": true, "searchable": true },
                {
                    title: 'Actions', data: null, class: "text-center clsWrap noExport", "orderable": false, "searchable": false,
                    render: function (data, type, row, meta) {
                        return UserActions(data.userId);
                    }
                }
            ];

            ApplyDatatableWithData("tblUserList", columns, result.data);
            $.unblockUI();
        }
        else {
            Toast.fire({ icon: 'error', title: result.message });
            $.unblockUI();
        }
        $('#tblUserList').on('click', '.btn-user-download', function () {
            $.blockUI();
            ajaxCall("Get", false, '/Report/DownloadReport?IsUserWiseReport=true&ProjectId=0&UserId=' + $(this).attr('UserId') + '&FromDate=' + $("#txtUserFromDate").val() + '&ToDate=' + $("#txtUserToDate").val(), null, function (result) {
                if (result.status == true) {
                    // If the result is successful and contains the file path
                    var filePath = result.data; // Assuming result contains a field 'filePath'

                    // Open the file in a new tab
                    var newTab = window.open(filePath, '_blank');

                    // Optional: Check if the tab was successfully opened
                    if (newTab) {
                        newTab.focus(); // Brings the new tab into focus
                    } else {
                        Toast.fire({ icon: 'error', title: 'Unable to open the file. Please check your browser settings.' });
                    }
                }
                else {
                    Toast.fire({ icon: 'error', title: 'Error while downloading the file' });
                }
                $.unblockUI();
            });
        });

    });
}
function Actions(id) {
    return '<i class="fa fa-download text-info btn-download my-pointer" ProjectId="' + id + '" title="Download"> Download</i>'
}

function UserActions(id) {
    return '<i class="fa fa-download text-info btn-user-download my-pointer" UserId="' + id + '" title="Download"> Download</i>'
}