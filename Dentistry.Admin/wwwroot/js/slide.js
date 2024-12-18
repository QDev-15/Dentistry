$(document).ready(function () {
    // Initialize DataTable
    const slideTable = $('#dataTableSlide').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });
    // handle Edit - Add button click
    $(document).on('click', '.slide-edit-btn, .slide-add-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        debugger;
        $.ajax({
            url: `/Slide/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#editSlideModal .modal-content').html(html);
                $('#editSlideModal').modal('show');
            },
            error: function () {
                alert('Failed to load data');
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
                $('#editSlideModal').modal('hide');
                location.reload(); // Hoặc cập nhật bảng
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