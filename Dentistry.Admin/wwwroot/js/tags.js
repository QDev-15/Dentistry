$(document).ready(function () {
    $('#tagsTable').DataTable({
        autoWidth: false,
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });

    // open model edit-add
    $(document).on('click', '.add-btn, .edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Tags/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditTagsModal .modal-content').html(html);
                $('#addEditTagsModal').modal('show');
            },
            error: function () {
                showError('Thất bại, xin vui lòng thử lại.');
            }
        });
    });
    // submit modal
    $('#addEditTagsModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        $.ajax({
            url: '/Tags/AddEdit',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addEditTagsModal').modal('hide');
                    loadTagsList();
                } else {
                    showInfo(response.message);
                }
            },
            error: function (err) {
                showError('Lưu thất bại.');
            }
        });
    });
});





