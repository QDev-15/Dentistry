
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
    $(document).on('click', '.contact-accept-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
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
        });
    });
    // handle Edit - Add button click
    $(document).on('click', '.contact-view-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Contact/View/${id}`,
            type: 'GET',
            success: function (html) {
                $('#viewContactModal .modal-content').html(html);
                $('#viewContactModal').modal('show');
            },
            error: function (err) {
                showError('Failed to load data');
            }
        });
    });
    // save contact
    $(document).on('click', '.contact-save', function () {
        const form = $("#viewContactForm");
        const formData = new FormData(form[0]);
        saveContact("Contact/Save", formData);
    });            
    // accept contact
    $(document).on('click', '.contact-accept', function () {
        $("#contact-isactive").val(false);
        const form = $("#viewContactForm");
        const formData = new FormData(form[0]);
        saveContact("Contact/Save", formData);

    });

    function saveContact(url, formData) {
        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#viewContactModal').modal('hide');
                    reloadData(); // Hoặc cập nhật bảng
                } else {
                    window.alert(response.message);
                }

            },
            error: function () {
                alert('Failed to save changes');
            }
        });
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
