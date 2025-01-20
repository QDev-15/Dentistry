
$(document).ready(function () {

    //$('#addContactMessage').on('submit', function (e) {
    //    e.preventDefault();
    //    const formData = new FormData(this);
    //    var form = $(this);

    //    $.ajax({
    //        url: '/Contact/AddMessage',
    //        type: 'POST',
    //        data: formData,
    //        processData: false,
    //        contentType: false,
    //        success: function (response) {
    //            if (!response.success) {
    //                // Hiển thị lỗi validation
    //                const errors = response.errors || [];
    //                $(this).find(".text-danger").empty(); // Xóa lỗi cũ

    //                errors.forEach(function (error, index) {
    //                    const input = form.find(`[name='${Object.keys(response.data)[index]}']`);
    //                    const errorSpan = input.next(".text-danger");
    //                    errorSpan.text(error);
    //                });
    //            } else {
    //                alert("Gửi thông tin thành công!");
    //            }
    //        },
    //        error: function (err) {
    //            alert('Lưu thất bại.');
    //        }
    //    });
    //});
});
