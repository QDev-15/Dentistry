
$(document).ready(function () {
    $(document).on('submit', '#addContactMessage', function (e) {
        e.preventDefault();
        var form = $(this);
        var $btn = form.find('button[type="submit"]');

        // Kiểm tra form hợp lệ TRƯỚC - nếu trống/sai thì dừng lại ngay, để jQuery Validate
        // tự hiện thông báo lỗi tại chỗ, không khoá nút/không gửi request gì cả.
        if (form.valid && !form.valid()) {
            return;
        }

        // Chặn bấm nhiều lần khi mạng chậm: khoá nút + đổi chữ ngay khi bấm, chỉ mở lại
        // khi có phản hồi (dù thành công hay lỗi) - tránh gửi trùng nhiều tin nhắn/email.
        if ($btn.prop('disabled')) {
            return;
        }
        var originalText = $btn.text();
        $btn.prop('disabled', true).text('Đang gửi...');

        const formData = new FormData(this);

        $.ajax({
            url: '/Contact/AddMessage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (!response.isSuccessed) {
                    // Hiển thị lỗi validation
                    if (response.data) {
                        $("#add_contact_message").html(response.data);
                        parseUnobtrusiveValidation('#add_contact_message');
                    }
                } else {
                    form[0].reset();
                    $("#close_button_modalOnloadDefaul").click();
                    showSuccess("Gửi thông tin thành công!");
                    addAddressLoading();
                }
            },
            error: function (err) {
                alert('Lưu thất bại.');
            },
            complete: function () {
                $btn.prop('disabled', false).text(originalText);
            }
        });
    });
});
function addAddressLoading() {
    $.ajax({
        url: '/Contact/AddMessage',
        type: 'Get',
        processData: false,
        contentType: false,
        success: function (response) {
            $("#add_contact_message").html(response);
            parseUnobtrusiveValidation('#add_contact_message');
        },
        error: function (err) {
            showError(error);
        }
    });
}