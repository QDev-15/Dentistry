
function initDatePicker() {
    if ($('.input-group-datetimepicker').length) {
        const bookContactDatePicker = new tempusDominus.TempusDominus($('.input-group-datetimepicker')[0], {
            display: {
                components: {
                    calendar: true,
                    date: true,
                    month: true,
                    year: true,
                    clock: false,
                },
            },
            localization: {
                locale: 'vi',
                format: 'dd/MM/yyyy'
            },
            useCurrent: true,
            restrictions: {
                minDate: new Date(),
                maxDate: new Date(new Date().setDate(new Date().getDate() + 365))
            }
        });

        // Gán lại sự kiện click icon lịch
        $(document).off('click', '#calendarIcon').on('click', '#calendarIcon', function () {
            if (bookContactDatePicker) {
                bookContactDatePicker.show();
            }
        });
    }
}
/// load form book contact for Modal
function bookFormLoading() {
    $.ajax({
        url: '/Contact/LoadBookForm',
        type: 'Get',
        processData: false,
        contentType: false,
        success: function (response) {
            $("#app-contact-content").html(response);
            initDatePicker();
            parseUnobtrusiveValidation('#app-contact-content');
        },
        error: function (err) {
            showError(err);
        }
    });
}

$(document).ready(function () {

    initDatePicker();

    $(document).on('submit', '#frmBook', function (e) {
        e.preventDefault();
        var form = $(this);
        var $btn = form.find('button[type="submit"]');

        // Kiểm tra form hợp lệ TRƯỚC - nếu trống/sai thì dừng lại ngay, để jQuery Validate
        // tự hiện thông báo lỗi tại chỗ, không khoá nút/không gửi request gì cả.
        if (form.valid && !form.valid()) {
            return;
        }

        // Chặn bấm nhiều lần khi mạng chậm: khoá nút + đổi chữ ngay khi bấm, chỉ mở lại
        // khi có phản hồi (dù thành công hay lỗi) - tránh tạo trùng nhiều lịch hẹn/email.
        if ($btn.prop('disabled')) {
            return;
        }
        var originalText = $btn.text();
        $btn.prop('disabled', true).text('Đang gửi...');

        const formData = new FormData(this);

        $.ajax({
            url: '/Contact/Book',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (!response.isSuccessed) {
                    // Hiển thị lỗi validation
                    if (response) {
                        $("#app-contact-content").html(response);
                        initDatePicker();
                        parseUnobtrusiveValidation('#app-contact-content');
                    }
                } else {
                    showSuccess("Gửi thông tin thành công!");
                    bookFormLoading();
                }
            },
            error: function (err) {
                showError('Lưu thất bại.');
            },
            complete: function () {
                $btn.prop('disabled', false).text(originalText);
            }
        });
    });
    
});
