function Enroll(id){
    $.ajax({
        type: "POST",
        url: window.location.href + '/',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data){
            if(data.success){
                toastr.success(data.message);
                location.reload();
                
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
                url: "/Authenticated/Enroll/Delete/" + idUrl,
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data){
                    if(data.success){
                        toastr.success(data.message);
                        location.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

$(document).ready(function(){
    $("#myInput").on("keyup", function() {
        var value = $(this).val().toLowerCase();
        $("#tblData tr").filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
