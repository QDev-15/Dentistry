
$(document).ready(function () {
    $('#contactActiveTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });
    $('#contactInActiveTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });
    function process(id) {
        showConfirm("Hoàn tất yêu cầu?", "Xác nhận").then(function (resp) {
            if (resp) {
                $.ajax({
                    url: '/Contact/Process?id=' + id,
                    type: 'GET',
                    success: function (data) {
                        reloadData();
                        showInfo("Hoàn thành.");
                    },
                    error: function (xhr, status, error) {
                        console.error("Error reloading doctor list:", error);
                    }
                });
            }
        })
    }
    function reloadData() {
        $.ajax({
            url: '/Contact/List',
            type: 'GET',
            success: function (data) {
                $('#section-contact').html(data);
            },
            error: function (xhr, status, error) {
                console.error("Error reloading contact list:", error);
            }
        });
    }
});
