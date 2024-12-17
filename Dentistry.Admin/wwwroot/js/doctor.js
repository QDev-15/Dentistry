function reloadDoctorList() {
    $.ajax({
        url: '/Doctor/List',
        type: 'GET',
        success: function (data) {
            $('#doctor').html(data);
        },
        error: function (xhr, status, error) {
            console.error("Error reloading doctor list:", error);
        }
    });
}