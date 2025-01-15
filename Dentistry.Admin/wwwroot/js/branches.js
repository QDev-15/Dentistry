$(document).ready(function () {
    $('#branchesTable').DataTable({
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
            url: `/Branches/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditBranchesModal .modal-content').html(html);
                $('#addEditBranchesModal').modal('show');
            },
            error: function () {
                showError('Thất bại, xin vui lòng thử lại.');
            }
        });
    });
    // change status branches
    $(document).on('click', '.activated-btn, .deactivated-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        const active = $(this).data('IsActive') || false; // Nếu không có ID, thì tạo mới
        $.ajax({
            url: '/Branches/Activated',
            type: 'POST',
            data: {
                active: active,
                id: id
            },
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addEditBranchesModal').modal('hide');
                    reloadBranchesList();
                } else {
                    showInfo(response.message);
                }
            },
            error: function (err) {
                showError('Lưu thất bại.');
            }
        });
    });
    // submit modal
    $('#addEditBranchesModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        $.ajax({
            url: '/Branches/AddEdit',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addEditBranchesModal').modal('hide');
                    reloadBranchesList();
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




function reloadBranchesList() {
    $.ajax({
        url: '/Branches/List',
        type: 'GET',
        success: function (data) {
            $('#branches').html(data);
        },
        error: function (xhr, status, error) {
            console.error("Error reloading branches list:", error);
        }
    });
}
