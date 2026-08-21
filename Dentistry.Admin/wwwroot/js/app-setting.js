$(document).ready(function () {
    var selectedAllIds = {

    }
    // submit modal
    $(document).on('submit', '#updateForm', function (e) {
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

    // preview ảnh trang chủ - tab dán link ảnh
    $(document).on('click', '#btn-preview-home-image-url', function () {
        const url = $('#homeImageUrlInput').val().trim();
        if (!url) return;
        $('#home-image-unsplash-results .unsplash-thumb').removeClass('selected');
        $('#homeImageUrlHidden').val(url);
        $('#home-image-preview').attr('src', url);
        $('#home-image-preview-container').show();
    });

    // tìm kiếm ảnh trên Unsplash cho ảnh trang chủ
    $(document).on('click', '#btn-search-home-image-unsplash', function () {
        const query = $('#homeImageUnsplashQuery').val().trim();
        if (!query) return;
        const $results = $('#home-image-unsplash-results');
        $results.html('<span>Đang tìm kiếm...</span>');
        $.ajax({
            url: '/AppSetting/SearchUnsplash',
            type: 'GET',
            data: { query: query },
            success: function (photos) {
                $results.empty();
                if (!photos || photos.length === 0) {
                    $results.html('<span>Không tìm thấy ảnh phù hợp.</span>');
                    return;
                }
                photos.forEach(function (photo) {
                    const $img = $('<img>')
                        .addClass('unsplash-thumb')
                        .attr('src', photo.thumb)
                        .attr('title', photo.description || '')
                        .attr('data-full', photo.regular);
                    $results.append($img);
                });
            },
            error: function () {
                $results.html('<span>Tìm kiếm thất bại, vui lòng thử lại.</span>');
            }
        });
    });

    // chọn 1 ảnh Unsplash làm ảnh trang chủ
    $(document).on('click', '#home-image-unsplash-results .unsplash-thumb', function () {
        $('#home-image-unsplash-results .unsplash-thumb').removeClass('selected');
        $(this).addClass('selected');
        const fullUrl = $(this).data('full');
        $('#homeImageUrlInput').val('');
        $('#homeImageUrlHidden').val(fullUrl);
        $('#home-image-preview').attr('src', fullUrl);
        $('#home-image-preview-container').show();
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
            },
            drawCallback: function () {
                if (!selectedAllIds[type]) {
                    selectedAllIds[type] = new Set();
                    var ids = $("#" + type + "IdsInput").val();
                    if (ids) {
                        ids = ids.split(",");
                        ids.forEach((id) => {
                            selectedAllIds[type].add(id);
                        })
                    }
                }
                if (!selectedAllIds[type + "Ids"]) {
                    selectedAllIds[type + "Ids"] = $("#" + type + "IdsInput");
                }
                // Khi vẽ lại bảng (sang trang khác), đảm bảo giữ trạng thái checkbox
                $("." + type + "-checkbox").each(function () {
                    const categoryId = $(this).val();
                    $(this).prop("checked", selectedAllIds[type].has(categoryId));
                });
            }
        });

        const typeIdsInput = $("#" + type + "IdsInput");

        // Lắng nghe sự kiện checkbox thay đổi
        $(document).on("change", "." + type + "-checkbox", function () {
            if (!selectedAllIds[type]) {
                selectedAllIds[type] = new Set();
                var ids = $("#" + type + "IdsInput").val();
                if (ids) {
                    ids = ids.split(",");
                    ids.forEach((id) => {
                        selectedAllIds[type].add(id);
                    })
                }
            }
            const categoryId = $(this).val();
            if (!selectedAllIds[type + "Ids"]) {
                selectedAllIds[type + "Ids"] = $("#" + type + "IdsInput");
            }
            if ($(this).prop("checked")) {
                selectedAllIds[type].add(categoryId); // Thêm ID vào danh sách
            } else {
                selectedAllIds[type].delete(categoryId); // Xóa ID khỏi danh sách
            }

            // Cập nhật input ẩn để lưu toàn bộ ID đã chọn
            selectedAllIds[type + "Ids"].val(Array.from(selectedAllIds[type]).join(","));
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

    // ======================================= Category list =============================================
    initTableList('categoryProduct');

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