var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/exams/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "surname", "width": "20%" },
            { "data": "subject", "width": "20%" },
            { "data": "grade", "width": "20%" },
            { "data": "speciality", "width": "20%" },
            { "data": "teacher", "width": "20%" }
       
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

