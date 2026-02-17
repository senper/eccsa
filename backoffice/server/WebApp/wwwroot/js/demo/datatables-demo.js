// Call the dataTables jQuery plugin
var options = {
    aLengthMenu: [
        [10, 25, 50, 100, -1],
        [10, 25, 50, 100, "All"]
    ],
};
$(document).ready(function () {
    if (!$.fn.DataTable.isDataTable('#dataTable')){
        $('#dataTable').DataTable(options);
    }
});

$(document).ready(function () {
    if (!$.fn.DataTable.isDataTable('.dataTable')) {
        $('.dataTable').DataTable(options);
    }
});
