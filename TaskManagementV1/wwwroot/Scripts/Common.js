var methodType = "Post";
var baseURL = "http://localhost:7063/";
var DataType = "json";
var isDataTypeJson = false;

document.addEventListener('DOMContentLoaded', function () {
    document.body.classList.add('fade-in');
});

var Toast = Swal.mixin({
    toast: true,
    //showCancelButton: true,
    position: 'top-end',
    //position: 'center',
    showConfirmButton: false,
    timer: 5000
});

var Popup_Toast = Swal.mixin({
    position: 'center',
    //icon: 'success',
    //title: 'Your work has been saved',
    showConfirmButton: false,
    allowOutsideClick: false,
    timer: 5000
})

function RedirectToPage(path) {
    //window.location.href = path;
    document.body.classList.add('fade-out');
    window.setTimeout(function () {
        window.location.href = path;
        $.unblockUI();
    }, 2000);
}

// General function for all ajax calls
function ajaxCall(methodType, applyBaseURL, apiURL, dataParams, callback) {
    var URL;
    if (applyBaseURL == true) {
        URL = baseURL + apiURL;
    }
    else {
        URL = apiURL;
    }

    $.ajax({
        type: methodType,
        url: URL,
        //quietMillis: 100,
        headers: {
            //'Authorization': Token,
            //"Content-Type": "application/json"
            'Access-Control-Allow-Origin': '*'
        },
        contentType: 'application/json; charset=utf-8',
        data: dataParams,
        dataType: DataType,
        cache: false,
        success: function (response) {
            callback(response);
        },
        error: function (response) {
            callback(response);
        }
    });
}


// General function for all ajax calls
function ajaxCallWithoutDataType(methodType, applyBaseURL, apiURL, dataParams, callback) {
    //var Token = $("#txtToken").val();
    var URL;
    if (applyBaseURL == true) {
        URL = baseURL + apiURL;
    }
    else {
        URL = apiURL;
    }

    $.ajax({
        type: methodType,
        url: URL,
        //quietMillis: 100,
        headers: {
            //'Authorization': Token,
            //"Content-Type": "application/json"
            /* 'Access-Control-Allow-Origin': '*'*/
        },
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: dataParams,
        /* dataType: DataType,*/
        //cache: false,
        success: function (response) {
            callback(response);
        },
        error: function (response) {
            callback(response);
        }
    });
}
function ajaxCallDatatable(methodType, applyBaseURL, apiURL, dataParams, callback) {
    var URL;
    if (applyBaseURL == true) {
        URL = baseURL + apiURL;
    }
    else {
        URL = apiURL;
    }
    $.ajax({
        type: methodType,
        url: URL,
        //quietMillis: 100,
        headers: {
            //'Authorization': Token,
            //"Content-Type": "application/json"
            'Access-Control-Allow-Origin': '*'
        },
        contentType: 'application/json; charset=utf-8',
        data: dataParams,
        dataType: DataType,
        cache: false,
        success: function (response) {
            callback(response);
        },
        error: function (response) {
            callback(response);
        }
    });
}

function ApplyDatatableWithData(id, columns, data) {
    datatableId = "#" + id;
    datatableWrapper = datatableId + "_wrapper";
    var table = $(datatableId).dataTable({
        //"responsive": true,
        "lengthchange": true,
        "paging": true,
        "searching": true,
        "processing": true,
        "ordering": true,
        //"order": [0, 'asc'],
        "order": [],
        //"aaSorting": [[1, "asc"]],
        "destroy": true,
        /*"autoWidth": false,*/
        //"scrollX": false,
        //"scrollY": "35vh",
        //"scrollCollapse": true,

        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'All']],
        //fixedHeader: true,
        //"pageResize": true,
        //"serverSide": true,
        //dom: 'lBfrtip',
        "dom": 'l<"row"<"col-md-6"B><"col-md-6"f>><"row form-group"<"col-md-12"rt>><"row"<"col-md-6"i><"col-md-6"p>>',
        //"dom": 'l<"row"<"col-md-6"B><"col-md-6"f>>rt<"row"<"col-md-6"i><"col-md-6"p>>',
        //dom: 'l<"container-fluid"<"row"<"col"B><"col"f>>>rtip',
        //"dom": 'Bfrtip',
        //"dom": "<'row'<'form-inline' <'col-sm-offset-5'B>>>"
        //    + "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>"
        //    + "<'row'<'col-sm-12'tr>>"
        //    + "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        //"dom": "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
        //    "<'row'<'col-sm-12'tr>>" +
        //    "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        //"columnDefs": columnDefs,
        data: data,
        buttons: [
            {
                extend: 'copy', className: 'btn-primary',
                exportOptions: { columns: "thead tr th:not(.noExport)" },
                init: function (api, node, config) {
                    $(node).removeClass('btn-secondary')
                }
            },
            {
                extend: 'csv', className: 'btn-primary border border-white border-left-1 border-bottom-0 border-top-0',
                exportOptions: { columns: "thead tr th:not(.noExport)" },
                init: function (api, node, config) {
                    $(node).removeClass('btn-secondary')
                }
            },
            {
                extend: 'excel', className: 'btn-primary border border-white border-left-1 border-bottom-0 border-top-0',
                exportOptions: { columns: "thead tr th:not(.noExport)" },
                init: function (api, node, config) {
                    $(node).removeClass('btn-secondary')
                }
            },
            {
                extend: 'pdf', className: 'btn-primary border border-white border-left-1 border-bottom-0 border-top-0',
                exportOptions: { columns: "thead tr th:not(.noExport)" },
                init: function (api, node, config) {
                    $(node).removeClass('btn-secondary')
                },
                //orientation: 'landscape',
                //customize: function (doc) {
                //    doc.content[1].table.width = Array(doc.content[1].table.body[0].length + 1).join('*').split('');
                //}
                customize: function (doc) {
                    var colCount = new Array();
                    $(datatableId).find('tbody tr:first-child td').each(function () {
                        if ($(this).attr('colspan')) {
                            for (var i = 1; i <= $(this).attr('colspan'); $i++) {
                                colCount.push('*');
                            }
                        } else { colCount.push('*'); }
                    });
                    doc.content[1].table.widths = colCount;
                }
            },
            {
                extend: 'print', className: 'btn-primary border border-white border-left-1 border-bottom-0 border-top-0',
                exportOptions: { columns: "thead tr th:not(.noExport)" },
                init: function (api, node, config) {
                    $(node).removeClass('btn-secondary')
                }
            },
            {
                extend: 'colvis', className: 'btn-primary border border-white border-left-1 border-bottom-0 border-top-0',
                columns: ":not(.noVis)",
                init: function (api, node, config) {
                    $(node).removeClass('btn-secondary')
                }
            }
        ],
        columns: columns,
        "initComplete": function (settings, json) {
            $(datatableId).wrap("<div style='overflow:auto; width:100%;position:relative;'></div>");
        },
        //"initComplete": function (settings, json) {
        //    $(datatableId).wrap("<div style='width:100%;position:relative;'></div>");
        //},
        //scrollY: 200,
        //deferRender: true,
        //scroller: true,
        //customize: function (doc) {
        //    doc.content[1].table.body[1].forEach(function (h) {
        //        h.fillColor = 'green';
        //        alignment: 'center'
        //    });
        //}
    });
    /* table.columns.adjust().responsive.recalc();*/
    // listen for the length change event
    //table.on('length.dt', function (e, settings, len) {
    //    // recalculate the responsiveness after changing the page length
    //    //table.responsive.recalc();
    //});
    //$(datatableId).show();
    //$(window).trigger('resize');
    //.buttons().container().appendTo(datatableWrapper + ' .col-md-6:eq(0)');
    //$(datatableId).columns.adjust().draw();
    //$(datatableId).DataTable().columns.adjust().draw();
    //$("datatableId").append(table.table().container());


}


function ApplyDatatable(id) {
    datatableId = "#" + id;
    datatableWrapper = datatableId + "_wrapper";
    $(datatableId).DataTable({
        "responsive": true,
        "autoWidth": false,
        /*"scrollX": false,*/
        "scrollY": "35vh",
        "scrollCollapse": true,
        "lengthchange": true,
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'All']],
        //fixedHeader: true,
        /* dom: 'Bfrtip',*/
        buttons: [
            {
                extend: 'copy',
                exportOptions: { columns: "thead td:not(.noExport)" }
            },
            {
                extend: 'csv',
                exportOptions: { columns: "thead td:not(.noExport)" }
            },
            {
                extend: 'excel',
                exportOptions: { columns: "thead td:not(.noExport)" }
            },
            {
                extend: 'pdf',
                exportOptions: { columns: "thead td:not(.noExport)" },
                //orientation: 'landscape',
                customize: function (doc) {
                    doc.content[1].width = Array(doc.content[1].table.body[1].length + 1).join('*').split('');
                }
            },
            {
                extend: 'print',
                exportOptions: { columns: "thead td:not(.noExport)" }
            },
            {
                extend: 'colvis',
                columns: ':not(.noVis)'
            }
        ],
    }).buttons().container().appendTo(datatableWrapper + ' .col-md-6:eq(0)');
}

$(document).ready(function () {

    ApplyEvents();
});


function ApplyEvents() {
    $(".onlyNumeric").bind("paste", function () {
        onlyNumeric(event);
    });

    $(".onlyletter").bind("paste", function () {
        onlyletter(event);
    });

    $(".onlyNumeric").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            (e.keyCode === 86 && (e.ctrlKey === true || e.metaKey === true)) ||
            //Allow : Ctrl+X
            (e.keyCode === 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $('.onlyletter').keydown(function (e) {
        if (e.altKey) {
            e.preventDefault();
        }
        else {
            var key = e.keyCode;
            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key == 9))) {
                e.preventDefault();
            }
        }
    });
    $('.txtCapitalize').blur(function () {
        var str = $(this).val();
        var spart = str.split(" ");
        for (var i = 0; i < spart.length; i++) {
            var j = spart[i].charAt(0).toUpperCase();
            spart[i] = j + spart[i].substr(1);
        }
        $(this).val(spart.join(" "));
    });
    //$(".inputField").on("input", function () {
    //    var input = $(this).val();
    //    if (input.length != 1) {
    //        alert("Only one letter allowed!");
    //        $(this).val("");
    //    }
    //});

    $(".text-length").blur(function () {
        var obj = $(this);
        var val = $(obj).val().trim();
        if (val.length < $(obj).attr("minlength") || val.length > $(obj).attr("maxlength")) {
            var min = parseInt($(obj).attr("minlength"));
            var max = parseInt($(obj).attr("maxlength"));
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }
    });

    $('.OnlyNumberNotAllow').keydown(function (e) {
        if (e.altKey) {
            e.preventDefault();
        }
        else {
            var key = e.keyCode;
            if ((key >= 48 && key <= 57 && !e.shiftKey) || (key >= 96 && key <= 105)) {
                e.preventDefault();
            }
        }
    });

    $(".NumericWithSlash-dash").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 191, 111, 189, 109]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            (e.keyCode === 86 && (e.ctrlKey === true || e.metaKey === true)) ||
            //Allow : Ctrl+X
            (e.keyCode === 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $(".NumericWithdash").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 191, 189, 109]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            (e.keyCode === 86 && (e.ctrlKey === true || e.metaKey === true)) ||
            //Allow : Ctrl+X
            (e.keyCode === 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $(".NumericWithDot").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        //if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 191, 189, 109]) !== -1 ||
        //    // Allow: Ctrl+A, Command+A
        //    (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        //    // Allow: Ctrl+A, Command+A
        //    (e.keyCode === 67 && (e.ctrlKey === true || e.metaKey === true)) ||
        //    (e.keyCode === 86 && (e.ctrlKey === true || e.metaKey === true)) ||
        //    //Allow : Ctrl+X
        //    (e.keyCode === 88 && (e.ctrlKey === true || e.metaKey === true)) ||
        //    // Allow: home, end, left, right, down, up
        //    (e.keyCode >= 35 && e.keyCode <= 40)) {
        //    // let it happen, don't do anything
        //    return;
        //}
        //// Ensure that it is a number and stop the keypress
        //if ((e.shiftKey || ((e.keyCode < 48 && e.keycode != 46) || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        //    e.preventDefault();
        //}

        if (
            e.keyCode == 46 ||                               /* dot */
            e.keyCode == 110 ||                               /* dot */
            //e.keyCode == 190 ||                               /* dot */
            (e.keyCode >= 48 && e.keyCode <= 57) ||         /* number */
            (e.keyCode >= 96 && e.keyCode <= 105) ||         /* number keypad */
            e.keyCode == 27 ||                              /* esc */
            e.keyCode == 127 ||                             /* delete */
            e.keyCode == 8 ||                              /* backspace */
            e.keyCode == 9 ||                              /* tab */
            e.keyCode == 37 ||                              /* Left Arrow Key */
            e.keyCode == 39 ||                              /* Right Arrow Key */
            ((!e.shiftKey) && e.keyCode == 190)             /* dot */
        ) {
            if (e.keyCode == 110 || ((!e.shiftKey) && e.keyCode == 190)) {
                var stringValue = $(this).val();
                var charCount = {};
                for (let i = 0; i < stringValue.length; i++) {
                    let character = stringValue[i];
                    charCount[character] = (charCount[character] || 0) + 1;
                }
                if (charCount['.'] > 0) {
                    e.preventDefault();
                }
                else {
                    return
                }
            } else {
                return
            }
        }
        else {
            e.preventDefault();
        }
    });

    $('.txtuppercase').blur(function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('.txtlowercase').blur(function () {
        $(this).val($(this).val().toLowerCase());
    });

    $(".onlyEmail").blur(function () {
        var obj = $(this);
        var val = $(obj).val().trim();
        if ((obj.attr('isRequired') == '1' && !IsEmail(val)) || (obj.attr('isRequired') == '0' && val != "" && !IsEmail(val))) {
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }
    });

    $(".prevent-paste").on("paste", function (e) {
        e.preventDefault();
    })

    //Inputmask.extendAliases({
    //    'decimal': {
    //        integerDigits: 8,
    //        digits: 2,
    //        digitsOptional: false,
    //        numericInput: true,
    //        //autoGroup: true,
    //        //groupSeparator: ",",
    //        radixPoint: ".",
    //        placeholder: "0",
    //        rightAlign: false
    //    }
    //});

    $(".allowNumericLetters").keydown(function (e) {
        if (
            e.keyCode == 46 ||                               /* dot */
            //e.keyCode == 190 ||                               /* dot */
            //(e.keyCode >= 48 && e.keyCode <= 57) ||         /* number */
            (e.keyCode >= 96 && e.keyCode <= 105) ||         /* number keypad */
            (e.keyCode >= 65 && e.keyCode <= 90) ||         /* capital alphabet */
            (e.keyCode >= 97 && e.keyCode <= 122) ||         /* small alphabet */
            ((!e.shiftKey) && (e.keyCode >= 48 && e.keyCode <= 57)) ||
            e.keyCode == 27 ||                              /* esc */
            e.keyCode == 32 ||                              /* space */
            e.keyCode == 127 ||                             /* delete */
            e.keyCode == 8 ||                              /* backspace */
            e.keyCode == 9 ||                              /* tab */
            e.keyCode == 37 ||                              /* Left Arrow Key */
            e.keyCode == 39                              /* Right Arrow Key */
        ) {
            return
        }
        else {
            e.preventDefault();
        }
    });

    $("input").keypress(function (e) {
        var stringValue = $(this).val().trim();
        if (stringValue.length == 0 && e.keyCode == 32) {
            e.preventDefault()
        }
        else { return; }
    });

    $(".validate-datetime").blur(function () {
        var obj = $(this);
        var IsValidDateTimeRes = IsValidDateTime($(this).val(), $(this).attr('min'), $(this).attr('max'));
        if ((obj.attr('isRequired') == '1' && !IsValidDateTimeRes) || (obj.attr('isRequired') == '0' && val != "" && !IsValidDateTimeRes)) {
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }
    });
}

function validateRequiredFields() {
    $('.error').each(function (index, itm) {
        if (!$(itm).hasClass('d-none')) {
            $(itm).addClass('d-none');
        }
    });
    $('.has-error').each(function (index, itm) {
        if ($(this).hasClass('has-error')) {
            $(this).removeClass('has-error');
        }
    });

    $("[isRequired='1']").each(function (ind, item) {
        validateReqField($(this));
    });

    $("[isRequired='0']").each(function (ind, item) {
        if ($(this).val() != '') {
            validateReqField($(this));
        }
    });

    if ($('.has-error').length > 0) {
        $($($('.has-error').first()).find("input[isRequired='1']").first()).focus();
        return false;
    }
    return true;
}

function applyRequiredValidation() {
    $("[isRequired='1']").each(function (ind, item) {
        $(this).change(function () {
            validateReqField($(this));
        });
        $(this).blur(function () {
            validateReqField($(this));
        })
    });
}

function validateReqField(obj) {

    if (Array.isArray($(obj).val()) && $(obj).val().length === 0) {
        $("#" + $(obj).attr("errorspan")).removeClass('d-none');
        $("#" + $(obj).attr("divcontainer")).addClass('has-error');
        $("#" + $(obj).attr("id")).addClass('is-invalid');
        return
    }
    else if (!Array.isArray($(obj).val()) && $(obj).val().trim() == $(obj).attr("defaultvalue")) {
        $("#" + $(obj).attr("errorspan")).removeClass('d-none');
        $("#" + $(obj).attr("divcontainer")).addClass('has-error');
        $("#" + $(obj).attr("id")).addClass('is-invalid');
        return
    }
    else if ($(obj).hasClass('onlyEmail') && ((obj.attr('isRequired') == '1' && !IsEmail($(obj).val().trim())) || (obj.attr('isRequired') == '0' && $(obj).val().trim() != "" && !IsEmail($(obj).val().trim())))) {
        $("#" + $(obj).attr("errorspan")).removeClass('d-none');
        $("#" + $(obj).attr("divcontainer")).addClass('has-error');
        $("#" + $(obj).attr("id")).addClass('is-invalid');
        return
    }
    else if ($(obj).hasClass('validate-datetime') && ((obj.attr('isRequired') == '1' && !IsValidDateTime($(obj).val(), $(obj).attr('min'), $(obj).attr('max'))) || (obj.attr('isRequired') == '0' && $(obj).val().trim() != "" && !IsValidDateTime($(obj).val(), $(obj).attr('min'), $(obj).attr('max'))))) {
        $("#" + $(obj).attr("errorspan")).removeClass('d-none');
        $("#" + $(obj).attr("divcontainer")).addClass('has-error');
        $("#" + $(obj).attr("id")).addClass('is-invalid');
        return
    }
    //else if ($('.has-error').length > 0) {
    //    $("#" + $(obj).attr("errorspan")).removeClass('d-none');
    //    $("#" + $(obj).attr("divcontainer")).addClass('has-error');
    //    $("#" + $(obj).attr("id")).addClass('is-invalid');
    //    //return false;
    //}
    else {
        $("#" + $(obj).attr("errorspan")).addClass('d-none');
        $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
        $("#" + $(obj).attr("id")).removeClass('is-invalid');
    }

    var minLengthAttr = $(obj).attr('minlength');
    var maxLengthAttr = $(obj).attr('maxlength');
    if ((typeof minLengthAttr !== 'undefined' && minLengthAttr !== false) || (typeof maxLengthAttr !== 'undefined' && maxLengthAttr !== false)) {
        // checking min length value
        var min = parseInt(minLengthAttr);
        if ($(obj).val().length < min) {
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
            return
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }
        // checking max length value
        var max = parseInt(maxLengthAttr);
        if ($(obj).val().length > max) {
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
            return
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }
    }


    //if ($(obj).val().length < $(obj).attr("minlength") || $(obj).val().length > $(obj).attr("maxlength")) {
    //    var min = parseInt($(obj).attr("minlength"));
    //    var max = parseInt($(obj).attr("maxlength"));


    //    $("#" + $(obj).attr("errorspan")).removeClass('d-none');
    //    $("#" + $(obj).attr("divcontainer")).addClass('has-error');
    //    $("#" + $(obj).attr("id")).addClass('is-invalid');

    //    //if (min > 0 && max > 0 && min == max && min != $(obj).val().length) {
    //    //    $("#" + $(obj).attr("errorspan")).removeClass('d-none');
    //    //    $("#" + $(obj).attr("divcontainer")).addClass('has-error');
    //    //}
    //    //else if (min > 0 && max > 0 && min != max && !(min <= $(obj).val().length && max >= $(obj).val().length)) {
    //    //    $("#" + $(obj).attr("errorspan")).removeClass('d-none');
    //    //    $("#" + $(obj).attr("divcontainer")).addClass('has-error');
    //    //}
    //    //else {
    //    //    $("#" + $(obj).attr("errorspan")).addClass('d-none');
    //    //    $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
    //    //}
    //}

    var minAttr = $(obj).attr('min');
    var maxAttr = $(obj).attr('max');
    if ((typeof minAttr !== 'undefined' && minAttr !== false) || (typeof maxAttr !== 'undefined' && maxAttr !== false)) {
        var min = parseInt(minAttr);
        if ($(obj).val() < min) {
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
            return
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }

        var max = parseInt(maxAttr);
        if ($(obj).val() > max) {
            $("#" + $(obj).attr("errorspan")).removeClass('d-none');
            $("#" + $(obj).attr("divcontainer")).addClass('has-error');
            $("#" + $(obj).attr("id")).addClass('is-invalid');
            return
        }
        else {
            $("#" + $(obj).attr("errorspan")).addClass('d-none');
            $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
            $("#" + $(obj).attr("id")).removeClass('is-invalid');
        }
    }

    //else {
    //    $("#" + $(obj).attr("errorspan")).addClass('d-none');
    //    $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
    //    $("#" + $(obj).attr("id")).removeClass('is-invalid');
    //}

    //if (!IsEmail($(obj).val())) {
    //    $("#" + $(obj).attr("errorspan")).removeClass('d-none');
    //    $("#" + $(obj).attr("divcontainer")).addClass('has-error');
    //    $("#" + $(obj).attr("id")).addClass('is-invalid');
    //}
    //else {
    //    $("#" + $(obj).attr("errorspan")).addClass('d-none');
    //    $("#" + $(obj).attr("divcontainer")).removeClass('has-error');
    //    $("#" + $(obj).attr("id")).removeClass('is-invalid');
    //}
}

function GetFormattedDateString(date) {
    var d = new Date(date);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date = day + "/" + month + "/" + year;
    return date;
}
function IsEmail(email) {
    var regex = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    ///^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    }
    else {
        return true;
    }
}