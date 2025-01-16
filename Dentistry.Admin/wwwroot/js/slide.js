$(document).ready(function () {
    // Initialize DataTable
    const slideTable = $('#dataTableSlide').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
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
    $('#editSlideModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#editSlideModal').modal('hide');
                    loadSlideList(); // Hoặc cập nhật bảng
                }
            },
            error: function (error) {
                showError(error);
            }
        });
    });

    // Delete Confirm action
    $('#deleteSlideConfirm').on('click', function (e) {
        e.preventDefault();
        var id = $('#deleteId').val();
        $.ajax({
            url: `/Slide/Delete/${id}`,
            type: 'Delete',
            success: function (response) {
                if (response.isSuccessed) {
                    showSuccess("Xóa slide thành công");
                    loadSlideList();
                }
            },
            error: function (error) {
                showError(error);
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