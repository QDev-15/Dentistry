$(document).ready(function () {
    $('#btn-setting-update').on('click', function (e) {
        e.preventDefault();
        // Lấy dữ liệu từ form
        var formData = $('#updateForm').serialize();

        // AJAX request
        $.ajax({
            url: '/AppSetting/UpdateSetting', // Đường dẫn đến action
            method: 'POST',
            data: formData,
            success: function (response) {
                // Hiển thị thông báo thành công
                if (response.isSuccessed) {
                    loadData();
                    showSuccess("Thành công.");
                }
                
            },
            error: function (xhr, status, error) {
                // Hiển thị thông báo lỗi
                $('#responseMessage').html(
                    '<div class="alert alert-danger">Đã xảy ra lỗi: ' + xhr.responseText + '</div>'
                );
            },
        });
    });
    function loadData() {
        $.ajax({
            url: `/AppSetting/GetSetting`,
            type: 'GET',
            success: function (html) {
                $('#app-setting').html(html);
            },
            error: function () {
                showError('Tải lại cài đặt thất bại!');
            }
        });
    }
});