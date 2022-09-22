var dataTable;

$(document).ready(function () {

    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "ordering": true,
        "columnDefs": [{ "targets": 0, "type": "date-eu" }],
        "columns": [
            { "data": "displayOrderDate" },
            { "data": "region" },
            { "data": "city" },
            { "data": "category" },
            { "data": "product" },
            { "data": "quantity" },
            { "data": "unitPrice" },
            { "data": "totalPrice" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="btn-group" role="group">
							<a href="/Admin/Order/Edit?id=${data}" class="btn btn-primary mx-2">
							<i class="bi bi-pencil-square">Edit</i></a>
							<a onclick="Delete('/Admin/Order/Delete/${data}')" class="btn btn-danger mx-2">
							<i class="bi bi-trash-fill">Delete</i></a>
					</div>`;
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong!',
                            footer: '<a href="">Why do I have this issue?</a>'
                        });
                    }
                }
            });

        }
    })
}


var minDate, maxDate;

// Custom filtering function which will search data in column four between two values
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var min = minDate.val();
        var max = maxDate.val();
        var date = new Date(data[0]);

        if (
            (min === null && max === null) ||
            (min === null && date <= max) ||
            (min <= date && max === null) ||
            (min <= date && date <= max)
        ) {
            return true;
        }
        return false;
    }
);

$(document).ready(function () {
    // Create date inputs
    minDate = new DateTime($('#min'), {
        format: 'MMMM Do YYYY'
    });
    maxDate = new DateTime($('#max'), {
        format: 'MMMM Do YYYY'
    });

    // DataTables initialisation
    var table = $('#tblData').DataTable();

    // Refilter the table
    $('#min, #max').on('change', function () {
        table.draw();
    });
});