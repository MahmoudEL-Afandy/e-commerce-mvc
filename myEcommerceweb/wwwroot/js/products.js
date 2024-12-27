var dataTable;

//Ready method means when the after the page get loade data  
$(document).ready(function () {
    loadeData();

});

// i will create the lOadeData();

function loadeData() {
    dataTable = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData"
        },
        "columns": [
            // this must be as the signature of the data that return from json
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },  
            {
                "data": "id",
                "render": function (data) {
                    // to return the a the ` must be in the same line of the return word 
                    return `
                    <a href="/Admin/Product/Edit/${data}" class="btn btn-success">Edit</a>
                    <a onClick=DeleteItem("/Admin/Product/Delete/${data}") class="btn btn-danger">Delete</a> 
                    `
                    // i active here in a delete (DeleteItem function and give it url of the delete method in the ProductController)


                }
            }

        ]



    });
}


// i implement functions for delete product sweetalert

function DeleteItem(Url) {
    // all this from sweetalert2 website 
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            // start my business logic , i add it
            $.ajax({
                url: Url,
                type: "Delete",
                success: function (data) { // data that return from the url 
                    if (data.success) {

                        dataTable.ajax.reload();
                        toaster.success(data.message);
                    }
                    else {
                        toaster.error(data.message);
                    }


                }



            });

            // End My Business logic 

            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}