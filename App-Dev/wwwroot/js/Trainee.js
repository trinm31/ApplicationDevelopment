var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/Enroll/GetTrainee"
        },
        "columns":[
            {"data": "name", "width": "30%"},
            {"data": "email", "width": "30%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a onclick=Enroll("${data}") class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-plus"></i> Enroll
                                </a> 
                            </div>
                    `;
                },"width":"40%"
            }
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}

function Enroll(id){
    $.ajax({
        type: "POST",
        url: window.location.href + '/',
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

