var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/users/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "firstName", "width": "20%" },
            { "data": "lastName", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "role", "width": "20%" },
            { "data": "state", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/users/find?ID=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Exams
                        </a>
                        &nbsp;
                        <a href="/users/change?ID=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Change
                        </a>

                        </div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });

}
