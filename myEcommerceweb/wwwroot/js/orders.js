var dataTable;

//Ready method means when the after the page get loade data  
$(document).ready(function () {
    loadeData();

});

// i will create the lOadeData();

function loadeData() {
    dataTable = $("#mytable").DataTable({
        /*"#mytable" this is a id value of the table that is in view that will get that from this file */
        "ajax": {
            "url": "/Admin/Order/GetData"
        },
        "columns": [
            // this must be as the signature of the data that return from json
            {"data":"id"},
            { "data": "name" },
            { "data": "phone" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            {
                "data": "id",
                "render": function (data) {
                    // to return the a the ` must be in the same line of the return word 
                    return `
                    <a href="/Admin/Order/Details?orderHId=${data}" class="btn btn-success">Details</a>
                    
                    `
                    // i active here in a delete (DeleteItem function and give it url of the delete method in the ProductController)


                }
            }

        ]



    });
}

