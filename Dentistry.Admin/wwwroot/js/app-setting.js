$(document).ready(function () {
    // submit modal
    $('#app-setting-form').on('submit', 'form', function (e) {
        e.preventDefault();
        var formData = $('#updateForm').serialize();
        // AJAX request
        $.ajax({
            url: '/AppSetting/UpdateSetting', // Đường dẫn đến action
            method: 'POST',
            data: formData,
            success: function (response) {
                // Hiển thị thông báo thành công
                if (response.isSuccessed) {
                    showSuccess("Thành công.");
                    loadSettingData();
                }

            },
            error: function (xhr, status, error) {
                // Hiển thị thông báo lỗi
                showError(xhr.responseText);
            },
        });
    });

    function initTableList(type) {
        $('#' + type +'TableList').DataTable({
            autoWidth: false,
            columnDefs: [
                {
                    targets: 0, // Cột đầu tiên (checkbox)
                    //className: 'setting-category-cell-checkbox', // Thêm class cố định
                    orderable: true, // Không cho phép sắp xếp cột này
                    render: function (data, type, row, meta) {
                        if (type === 'display') {
                            // Khi hiển thị trong bảng, trả lại HTML của checkbox
                            return data;
                        } else {
                            // Khi sắp xếp, dựa trên giá trị data-checkbox (0 hoặc 1)
                            const cell = meta.settings.aoData[meta.row].anCells[meta.col];
                            const dataCell = $(cell);
                            const checkbox = dataCell.find('input[type="checkbox"]');  // Tìm input checkbox trong td
                            const checkboxState = checkbox.prop('checked');  // Lấy trạng thái của checkbox (true hoặc false)
                            return checkboxState ? 1 : 0;  // Trả về giá trị 1 nếu checked, 0 nếu unchecked
                        }
                    }
                }
            ],
            order: [[0, 'desc']],
            language: {
                search: "Tìm kiếm:",
                lengthMenu: "Hiển thị _MENU_ mục",
                zeroRecords: "Không tìm thấy dữ liệu",
                info: "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
                infoEmpty: "Không có dữ liệu",
                paginate: {
                    first: "Đầu",
                    last: "Cuối",
                    next: "Sau",
                    previous: "Trước",
                },
            }
        });

        const typeIdsInput = $("#" + type + "IdsInput");

        // Lắng nghe sự kiện checkbox thay đổi
        $("." + type + "-checkbox").on("change", function () {
            const selectedIds = $("." + type + "-checkbox:checked")
                .map(function () {
                    return $(this).val();
                })
                .get(); // Lấy danh sách ID được chọn

            // Gán danh sách ID vào input ẩn
            typeIdsInput.val(selectedIds.join(","));
        });


        $('#setting_show_' + type).on('change', function () {
            if (this.checked) {
                $(".setting-" + type + "-body").show();
            } else {
                $(".setting-" + type + "-body").hide();
            }
        });
    }



    // ======================================= Category list =============================================
    initTableList('category');

    // ======================================= Doctor list =============================================
    initTableList('doctor');

    // ======================================= Articles list =============================================
    initTableList('article');

    // ======================================= News list =============================================
    initTableList('news');


    // ======================================= Products list =============================================
    initTableList('product');
    

    // ======================================= Feedback list =============================================
    initTableList('feedback');
});