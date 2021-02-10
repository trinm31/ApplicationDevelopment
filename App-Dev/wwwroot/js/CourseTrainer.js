var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax":{
            "url": "/Authenticated/API/CourseTrainer/GetAll",
        },
        "columns":[
            {"data": "name", "width": "30%"},
            {"data": "courseCategory.name", "width": "30%"},
            {
                "data": "id",
                "render": function (data){
                    return `<div class="text-center">
                                <a href="/Authenticated/API/CourseTrainer/CourseTrainer/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-plus"></i> Assign
                                </a>                             
                            </div>
                    `;
                },"width":"50%"
            }
        ],
        "language":{
            "emptyTable": "No data Found"
        },
        "width":"100%"
    });
}


