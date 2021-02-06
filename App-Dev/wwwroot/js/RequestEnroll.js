var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("GetEnrollList?status=inprocess");
    }
    else {
        if (url.includes("approve")) {
            loadDataTable("GetEnrollList?status=approve");
        }
        else {
            if (url.includes("rejected")) {
                loadDataTable("GetEnrollList?status=rejected");
            }
            else {
                loadDataTable("GetEnrollList?status=all");
            }
        }
       
    }
});


function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Authenticated/API/EnrollRequest/" + url
        },
        "columns": [
            { "data": "traineeProfile.name", "width": "15%" },
            { "data": "traineeProfile.email", "width": "15%" },
            { "data": "course.name", "width": "15%" },
            { "data": "enrollStatus", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Approve('${data}')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="far fa-check-circle"></i> 
                                </a>
                                <a onclick="Reject('${data}')" class="btn btn-danger text-white" style="cursor:pointer">
                                   <i class="far fa-window-close"></i>
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}

function Approve(id){
    $.ajax({
        type: "POST",
        url: '/Authenticated/API/EnrollRequest/Approve',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data){
            if(data.success){
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}

function Reject(id){
    $.ajax({
        type: "POST",
        url: '/Authenticated/API/EnrollRequest/Reject',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data){
            if(data.success){
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}