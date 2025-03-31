
$(document).ready(function () {
    $(document).on('submit', '#addContactMessage', function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        var form = $(this);

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
        },
        error: function (err) {
            showError(error);
        }
    });
}