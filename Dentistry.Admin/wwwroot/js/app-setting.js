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
                    loadData();
                }

            },
            error: function (xhr, status, error) {
                // Hiển thị thông báo lỗi
                showError(xhr.responseText);
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




    // ======================================= Category list =============================================
    $('#categoryTableList').DataTable({
        autoWidth: false,
        columnDefs: [
            {
                targets: 0, // Cột đầu tiên (checkbox)
                className: 'setting-cell-checkbox', // Thêm class cố định
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
    
    const listCategoryIdsInput = $("#listCategoryIdsInput");

    // Lắng nghe sự kiện checkbox thay đổi
    $(".category-checkbox").on("change", function () {
        const selectedIds = $(".category-checkbox:checked")
            .map(function () {
                return $(this).val();
            })
            .get(); // Lấy danh sách ID được chọn

        // Gán danh sách ID vào input ẩn
        listCategoryIdsInput.val(selectedIds.join(","));
    });


    $('#Setting_ShowCategoryList').on('change', function () {
        if (this.checked) {
            $(".setting-categories-body").show();
        } else {
            $(".setting-categories-body").hide();
        }
    });


    // ======================================= Doctor list =============================================
    $('#doctorTableList').DataTable({
        autoWidth: false,
        columnDefs: [
            {
                targets: 0, // Cột đầu tiên (checkbox)
                //className: 'setting-doctor-cell-checkbox', // Thêm class cố định
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

    const listDoctorIdsInput = $("#listDoctorIdsInput");

    // Lắng nghe sự kiện checkbox thay đổi
    $(".doctor-checkbox").on("change", function () {
        const selectedIds = $(".doctor-checkbox:checked")
            .map(function () {
                return $(this).val();
            })
            .get(); // Lấy danh sách ID được chọn

        // Gán danh sách ID vào input ẩn
        listDoctorIdsInput.val(selectedIds.join(","));
    });


    $('#Setting_ShowDoctorSlideList').on('change', function () {
        if (this.checked) {
            $(".setting-doctors-body").show();
        } else {
            $(".setting-doctors-body").hide();
        }
    });

    // ======================================= Articles list =============================================
    $('#articleTableList').DataTable({
        autoWidth: false,
        columnDefs: [
            {
                targets: 0, // Cột đầu tiên (checkbox)
                //className: 'setting-doctor-cell-checkbox', // Thêm class cố định
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

    const listArticleIdsInput = $("#listArticleIdsInput");

    // Lắng nghe sự kiện checkbox thay đổi
    $(".article-checkbox").on("change", function () {
        const selectedIds = $(".article-checkbox:checked")
            .map(function () {
                return $(this).val();
            })
            .get(); // Lấy danh sách ID được chọn

        // Gán danh sách ID vào input ẩn
        listArticleIdsInput.val(selectedIds.join(","));
    });


    $('#Setting_ShowArtileSlideList').on('change', function () {
        if (this.checked) {
            $(".setting-articles-body").show();
        } else {
            $(".setting-articles-body").hide();
        }
    });

    // ======================================= News list =============================================
    $('#newsTableList').DataTable({
        autoWidth: false,
        columnDefs: [
            {
                targets: 0, // Cột đầu tiên (checkbox)
                //className: 'setting-doctor-cell-checkbox', // Thêm class cố định
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

    const listNewsIdsInput = $("#listNewsIdsInput");

    // Lắng nghe sự kiện checkbox thay đổi
    $(".news-checkbox").on("change", function () {
        const selectedIds = $(".news-checkbox:checked")
            .map(function () {
                return $(this).val();
            })
            .get(); // Lấy danh sách ID được chọn

        // Gán danh sách ID vào input ẩn
        listNewsIdsInput.val(selectedIds.join(","));
    });


    $('#Setting_ShowNewsList').on('change', function () {
        if (this.checked) {
            $(".setting-news-body").show();
        } else {
            $(".setting-news-body").hide();
        }
    });


    // ======================================= Products list =============================================
    $('#productTableList').DataTable({
        autoWidth: false,
        columnDefs: [
            {
                targets: 0, // Cột đầu tiên (checkbox)
                //className: 'setting-doctor-cell-checkbox', // Thêm class cố định
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

    const listProductIdsInput = $("#listProductIdsInput");

    // Lắng nghe sự kiện checkbox thay đổi
    $(".product-checkbox").on("change", function () {
        const selectedIds = $(".product-checkbox:checked")
            .map(function () {
                return $(this).val();
            })
            .get(); // Lấy danh sách ID được chọn

        // Gán danh sách ID vào input ẩn
        listProductIdsInput.val(selectedIds.join(","));
    });


    $('#Setting_ShowProductList').on('change', function () {
        if (this.checked) {
            $(".setting-product-body").show();
        } else {
            $(".setting-product-body").hide();
        }
    });
});