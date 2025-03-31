
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
        },
        error: function (err) {
            showError(error);
        }
    });
}

$(document).ready(function () {

    initDatePicker();

    $(document).on('submit', '#frmBook', function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        var form = $(this);

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
                    }
                } else {
                    showSuccess("Gửi thông tin thành công!");
                    bookFormLoading();
                }
            },
            error: function (err) {
                showError('Lưu thất bại.');
            }
        });
    }); 
    
});
