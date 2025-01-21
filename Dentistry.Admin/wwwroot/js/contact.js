
$(document).ready(function () {
    $('#contactActiveTable').DataTable({
        paging: true,       // Bật phân trang
        searching: true,    // Bật tìm kiếm
        ordering: true,     // Bật sắp xếp
        responsive: true,   // Hỗ trợ responsive
        pageLength: 10,     // Số item mặc định hiển thị (ví dụ: 10 dòng)
        lengthMenu: [5, 10, 25, 50, 100], // Các tùy chọn cho số item hiển thị
    });
    $('#contactInActiveTable').DataTable({
        paging: true,       // Bật phân trang
        searching: true,    // Bật tìm kiếm
        ordering: true,     // Bật sắp xếp
        responsive: true,   // Hỗ trợ responsive
        pageLength: 10,     // Số item mặc định hiển thị (ví dụ: 10 dòng)
        lengthMenu: [5, 10, 25, 50, 100], // Các tùy chọn cho số item hiển thị
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
