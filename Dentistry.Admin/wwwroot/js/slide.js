$(document).ready(function () {
    // Initialize DataTable
    const slideTable = $('#dataTableSlide').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5, // Mặc định hiển thị 5 dòng
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Tất cả"]] // Các tùy chọn số dòng hiển thị
    });
    // Open Edit - Add button click
    $(document).on('click', '.slide-edit-btn, .slide-add-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        $.ajax({
            url: `/Slide/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#editSlideModal .modal-content').html(html);
                $('#editSlideModal').modal('show');
            },
            error: function (error) {
                showError(error);
            }
        });
    });
    // submit modal
    $(document).on('submit', '#slide-add-edit', function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        // Xóa file ra khỏi formData
        formData.delete("ImageFile");
        showGlobalSpinner();
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    const slideId = response.data.id; 
                    $('#editSlideModal').modal('hide');
                    loadSlideList(); // Hoặc cập nhật bảng
                    hideGlobalSpinner();
                    showSuccess("Thành công");
                    // Nếu có file ảnh, tiếp tục upload ảnh (Bước 2)
                    const fileInput = document.getElementById("ImageFile");
                    if (fileInput.files.length > 0) {
                        uploadSlideImage(slideId, fileInput.files[0]);
                    } else {
                        hideGlobalSpinner();
                    }
                }
            },
            error: function (error) {
                hideGlobalSpinner();
                showError(error);

            }
        });
    });

    // Delete Confirm action
    $('#deleteSlideConfirm').on('click', function (e) {
        e.preventDefault();
        var id = $('#deleteId').val();
        showGlobalSpinner();
        $.ajax({
            url: `/Slide/Delete/${id}`,
            type: 'Delete',
            success: function (response) {
                if (response.isSuccessed) {
                    showSuccess("Xóa slide thành công");
                    loadSlideList();
                    hideGlobalSpinner();
                }
            },
            error: function (error) {
                showError(error);
                hideGlobalSpinner();
            }
        });
    });

    const confirmDeleteModal = document.getElementById('confirmDeleteModal');
    confirmDeleteModal.addEventListener('show.bs.modal', function (event) {
        // Lấy nút đã kích hoạt modal
        const button = event.relatedTarget;
        // Lấy giá trị ID từ data-id của nút
        const id = button.getAttribute('data-id');
        // Gán ID vào input ẩn trong modal
        const deleteIdInput = document.getElementById('deleteId');
        deleteIdInput.value = id;
    });

});

// Hàm upload ảnh riêng biệt sau khi đã lưu slide thành công
function uploadSlideImage(slideId, file) {
    const formData = new FormData();
    formData.append("slideId", slideId);
    formData.append("imageFile", file);

    $.ajax({
        url: '/Slide/UploadImage',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.isSuccessed) {
                debugger;
                var data = response.data;
                $(".slide-avatar-" + data.id).css("opacity", 0).attr("src", data.image.path).on("load", function () {
                    $(this).fadeTo(500, 1); // Làm mờ dần trong 500ms
                });
                //$(".slide-avatar-" + data.id).attr('src', data.image.path);
                //showSuccess("Upload ảnh thành công");
            } else {
                showError("Upload ảnh thất bại");
            }
        },
        error: function (error) {
            showError("Lỗi khi upload ảnh");
        }
    });
}