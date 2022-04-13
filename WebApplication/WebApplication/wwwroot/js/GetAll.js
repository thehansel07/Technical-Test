$(document).ready(function () {

    $("#Table").DataTable({
        "processing": true, // for show progress bar    
        "serverSide": true, // for process server side    
        "filter": true, // this is for disable filter (search box)    
        "orderMulti": false, // for disable multiple column at once 
        "ajax": {
            "url": "/Books/LoadData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            {
                "data": "id", "name": "id", "autoWidth": true
            },
            {
                "data": "title", "name": "title", "autoWidth": true
            },
            {
                "data": "description", "name": "description", "autoWidth": true
            },
            {
                "data": "excerpt", "name": "excerpt", "autoWidth": true
            },
            {
                "data": "publishDate", "name": "publishDate", "autoWidth": true
            },
            {
                "render": function (data, type, full, meta, row) {
                    return '<a class="btn btn-warning" href="/Books/GetDatabyId/' + full.id + '">View Detail</a>'
                   


                }
            },
            {
                "render": function (data, type, full, meta, row) {

                    return '<a class="btn btn-danger" href="/Books/Delete/' + full.id + '">Delete</a>'


                }
            }


        ],

    });

});

$(document).on('click', '.btn-eliminar', function (event) {
    var json = $(this).data("informacion")
    var data=json.id;
    jQuery.ajax({
        url: 'Delete',
        type: "POST",
        data: JSON.stringify({ id: data }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data.resultado) {
                tabladata.ajax.reload();
            } else {
                alert("No se pudo eliminar")
            }
        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {

        },
    });
});









function abrirModal(json) {

    $("#txtId").val(0);
    $("#txtTitle").val("");
    $("#txtDescription").val("");
    $("#txtExcerpt").val("");
    $("#txtPublishDate").val("");
    $("#txtPageCount").val(0);
    $("#txtPublishDate").val("");


    if (json != null) {

        $("#txtId").val(json.txtId);
        $("#txtTitle").val(json.txtTitle);
        $("#txtDescription").val(json.txtDescription);
        $("#txtExcerpt").val(json.txtExcerpt);
        $("#txtPublishDate").val(json.txtPublishDate);
        $("#txtPageCount").val(json.txtPageCount);
        $("#txtPublishDate").val(json.txtPublishDate);

       
    }



    $('#FormModal').modal('show');
}


$(document).on('click', '.btn btn-info', function (event) {
    var json = $(this).data("informacion");
    abrirModal(json);
});

function CloseWindow() {
    $('#FormModal').modal('hide');

}

//Validation Data
function validation() {
    var result = true;
    if (document.getElementById("txtId").value == "0" || document.getElementById("txtId").value == "")
    {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'El Campo ID esta vacio!'
        })
        ;
        return result = false;
    }

    if (document.getElementById("txtTitle").value === "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'El Campo Title esta vacio!'
        })
        ;
        return result = false;
    }
    if (document.getElementById("txtDescription").value === "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'El Campo Description esta vacio!'
        })
        ;
        return result = false;
    }
    if (document.getElementById("txtExcerpt").value === "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'El Campo Excerpt esta vacio!'
        })
        ;
        return result = false;
    }
    if (document.getElementById("txtPublishDate").value === "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'El Campo PublishDate esta vacio!'
        })
        ;
        return result = false;
    }
    if (document.getElementById("txtPageCount").value == "0" || document.getElementById("txtPageCount").value == "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'El Campo PageCount esta vacio!'
        })
        ;
        return result = false;
    }

    return result;
}


function Guardar() {
    validation();
    var request = {
        objeto: {
            id: parseInt($("#txtId").val()),
            title: $("#txtTitle").val(),
            description: $("#txtDescription").val(),
            excerpt: $("#txtExcerpt").val(),
            publishDate: $("#txtPublishDate").val(),
            pageCount: parseInt($("#txtPageCount").val())
        }
    }
    var data = request.objeto;
    if (validation() == true) {

    $.ajax({
        type: "Post",
        url: "AddBooks",
        data: data
    });
        var table = $('#example').DataTable({
            ajax: "data.json"
        });

        setInterval(function () {
            table.ajax.reload(null, false); // user paging is not reset on reload
        }, 30000);

        Swal.fire({
            position: 'center',
            icon: 'success',
            title: 'Your Data has been saved',
            showConfirmButton: false,
            timer: 3000
        });

        $('#FormModal').modal('hide');
    }


}