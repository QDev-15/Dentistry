$(document).ready(function () {
    $('#branchesTable').DataTable({
        autoWidth: false,
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5,
        order: [[2, 'asc']]
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
                var message = id == 0 ? "Thêm mới thành công" : "Cập nhật thành công";
                showSuccess(message);
                reloadBranchesList();
            },
            error: function (error) {
                showError('Thất bại, xin vui lòng thử lại.');
            }
        });
    });
    // change status branches
    $(document).on('click', '.activated-btn, .deactivated-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        const active = $(this).data('isactive') || false; // Nếu không có ID, thì tạo mới
        $.ajax({
            url: '/Branches/Activated',
            type: 'POST',
            data: {
                id: id, active: active
            },
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (response) {
                if (response.isSuccessed) {
                    showSuccess("Cập nhật thành công.");
                    reloadBranchesList();
                } else {
                    showError(response.message);
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
