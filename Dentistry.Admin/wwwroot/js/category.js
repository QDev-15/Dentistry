﻿$(document).ready(function () {
    // Initialize DataTable
    $('#categoryTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });
    $('#subCategoryTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });
    // handle Edit - Add button click
    $(document).on('click', '.add-edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        const type = $(this).data('type'); // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Category/AddEdit/${id}?type=${type}`,
            type: 'GET',
            success: function (html) {
                console.log("html = " + html);
                $('#addEditCategoryModal .modal-content').html(html);
                $('#addEditCategoryModal').modal('show');
            },
            error: function () {
                alert('Failed to load data');
            }
        });
    });
    // submit modal
    $('#addEditCategoryModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                debugger;
                if (response.success) {
                    $('#addEditCategoryModal').modal('hide');
                    location.reload(); // Hoặc cập nhật bảng
                } else {
                    window.alert(response.message);
                }

            },
            error: function () {
                alert('Failed to save changes');
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