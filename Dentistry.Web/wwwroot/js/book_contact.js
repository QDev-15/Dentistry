
$(document).ready(function () {
    const picker = new tempusDominus.TempusDominus($('.input-group-datetimepicker')[0], {
        display: {
            components: {
                calendar: true,
                date: true,
                month: true,
                year: true,
                clock: false,
                //hours: false,
                //minutes: false,
                //seconds: false
            },
        },
        localization: {
            locale: 'vi',
            format: 'dd/MM/yyyy'
        },
        useCurrent: true,
        restrictions: {
            minDate: new Date(),
            maxDate: new Date(new Date().setDate(new Date().getDate() + 365))  // Giới hạn đến 365 ngày sau
        }
    });

    // Khi click vào icon lịch, mở DateTimePicker
    // Đảm bảo sự kiện không bị gán nhiều lần
    $('#calendarIcon').on('click', function () {
        console.log("show datepicker");
        picker.show();
    });

    function bookFormLoading() {
        $.ajax({
            url: '/Contact/LoadBookForm',
            type: 'Get',
            processData: false,
            contentType: false,
            success: function (response) {
                $("#app-contact-content").html(response);
            },
            error: function (err) {
                alert(error);
            }
        });
    }

    $('#frmBook').on('submit', function (e) {
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
                    if (response.data) {
                        $("#app-contact-content").html(response.data);
                    }
                } else {
                    form[0].reset();
                    $("#frmBook").find("input, textarea").val("");
                    $("#frmBook").find("select").val("0");
                    showSuccess("Gửi thông tin thành công!");
                }
            },
            error: function (err) {
                alert('Lưu thất bại.');
            }
        });
    }); 
    
});
