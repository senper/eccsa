function confirmDelete(form) {
    $.confirm({
        icon: 'fa fa-exclamation-triangle',
        title: 'Confirm!',
        content: 'Are you sure want to Delete ?',
        buttons: {
            yes: function () {
                form.submit();
            },
            no: function () {
            }
        }
    });
    return false;
}

function confirmation(form, text) {
    $.confirm({
        icon: 'fa fa-exclamation-triangle',
        title: 'Confirm!',
        content: 'Are you sure want to ' + text + ' ?',
        buttons: {
            yes: function () {
                form.submit();
            },
            no: function () {
            }
        }
    });
    return false;
}
