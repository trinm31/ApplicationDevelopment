var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/AssignToTrainer/GetTrainer"
        },
        "columns":[
            {"data": "name", "width": "30%"},
            {"data": "email", "width": "30%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a id="btn-${data}" onclick=Assign("${data}") class="btn btn-success text-white"  style="cursor: pointer">
                                    <i class="fas fa-plus"></i> Assign
                                </a> 
                                <a id="btnDelete-${data}" onclick=Delete("${data}") class="btn btn-danger text-white"  style="cursor: pointer">
                                    <i class="fas fa-trash-alt"></i> Delete
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

function Assign(id){
    $.ajax({
        type: "POST",
        url: window.location.href + '/',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data){
            if(data.success){
                //$("#stringId a").first().removeClass("btn btn-success text-white").addClass("btn btn-success text-white d-lg-none");
                toastr.success(data.message);
                // dataTable.rows()
                //     .invalidate()
                //     .draw();
                // dataTable.ajax.reload();
                var element = document.getElementById('btn-' + id);
                element.className = element.className.replace(/\bbtn btn-success text-white\b/g, "btn btn-success text-white d-lg-none");
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}

var url = window.location.href;
var idUrl = url.substring(url.lastIndexOf('/') + 1);

function Delete(id){
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be Enroll",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete){
            $.ajax({
                type: "POST",
                url: "/Authenticated/AssignToTrainer/Delete/" + idUrl,
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data){
                    if(data.success){
                        toastr.success(data.message);
                        var element = document.getElementById('btn-' + id);
                        element.className = element.className.replace(/\bbtn btn-success text-white d-lg-none\b/g, "btn btn-success text-white");
                        //dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
