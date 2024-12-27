var dataTable;
var id;

//Ready method means when the after the page get loade data  
$(document).ready(function () {
    loadeData();

});

// i will create the lOadeData();

function loadeData() {
    dataTable = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Users/GetData"
        },
        "columns": [
            // this must be as the signature of the data that return from json
            { "data": "name" },
            { "data": "email" },
            { "data": "address" },
            { "data": "city" },
            {
                "data": "id",
                //"render": function (data,row) {
                //    var DateTimenow = new Date();

                //    if (row.lockoutEnd == null || new Date(row.lockoutEnd).getTime() < DateTimenow.getTime()) {
                //        return `
                //            <a class="btn btn-success" asp-action="LockUnlock" asp-route-id="${data}"><i class="fas fa-lock-open"></i></a>
                //            <a onClick="DeleteItem('/Admin/Users/Delete/${data}')" class="btn btn-danger">Delete</a>
                //        `;
                //    } else {
                //        return `
                //            <a class="btn btn-success" asp-action="LockUnlock" asp-route-id="${data}"><i class="fas fa-lock"></i></a>
                //            <a onClick="DeleteItem('/Admin/Users/Delete/${data}')" class="btn btn-danger">Delete</a>
                //        `;
                //    }
                //}
                "render": function (data) {
                   id = data;
                    // to return the a the ` must be in the same line of the return word 
                    return `
                          <a onClick="DeleteItem('/Admin/Users/Delete/${data}')" class="btn btn-danger">Delete</a>
                         
                         

                        `
                    // i active here in a delete (DeleteItem function and give it url of the delete method in the ProductController)

                }
            },
            {
                "data": "lockoutEnd",
                "render": function (data) {
                    var DateTimenow = new Date();
                    

                    if (data == null || new Date(data) < DateTimenow) {
                        return `
                          <a onClick="Lock('/Admin/Users/LockUnlock/${id}')" class="btn btn-success"><i class="fas fa-lock-open"></i></a>
                          
                           
                        `
                    } else {
                        return `
                             <a class="btn btn-dark" onClick="UnLock('/Admin/Users/LockUnlock/${id}')"><i class="fas fa-lock"></i></a>


                        `
                    }
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



// Function to lock a user
function Lock(Url) {

    // all this from sweetalert2 website 
    Swal.fire({
        title: "Are you sure?",
        text: "You can be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Lock it!"
    }).then((result) => {
        if (result.isConfirmed) {
            // start my business logic , i add it
            $.ajax({
                url: Url,
                type: "POST",
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
                title: "Locked!",
                text: "The Account has been Locked.",
                icon: "success"
            });
        }
    });

    // Perform the lock user action here
    //$.ajax({
    //    url: "/Admin/Users/LockUnlock/" + id,
    //    type: "POST",
    //    success: function (data) {
    //        if (data.success) {
    //            dataTable.ajax.reload();
    //            toaster.success(data.message);
    //        } else {
    //            toaster.error(data.message);
    //        }
    //    }
    //    //,
    //    //error: function (xhr, status, error) {
    //    //    toaster.error("Error locking user. Please try again.");
    //    //}
    //});
}

function UnLock(Url) {

    // all this from sweetalert2 website 
    Swal.fire({
        title: "Are you sure?",
        text: "You can be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Un Lock it!"
    }).then((result) => {
        if (result.isConfirmed) {
            // start my business logic , i add it
            $.ajax({
                url: Url,
                type: "POST",
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
                title: "Un Locked!",
                text: "The Account has been Un Locked.",
                icon: "success"
            });
        }
    });

    // Perform the lock user action here
    //$.ajax({
    //    url: "/Admin/Users/LockUnlock/" + id,
    //    type: "POST",
    //    success: function (data) {
    //        if (data.success) {
    //            dataTable.ajax.reload();
    //            toaster.success(data.message);
    //        } else {
    //            toaster.error(data.message);
    //        }
    //    }
    //    //,
    //    //error: function (xhr, status, error) {
    //    //    toaster.error("Error locking user. Please try again.");
    //    //}
    //});
}