$(document).ready(function () {
    $('#doctorTable').DataTable({
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
            url: `/Doctor/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditDoctorModal .modal-content').html(html);
                $('#addEditDoctorModal').modal('show');
                initTiny("doctor_Description");
            },
            error: function () {
                showError('Thất bại, xin vui lòng thử lại.');
            }
        });
    });
    // submit modal
    $('#addEditDoctorModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        $.ajax({
            url: '/Doctor/AddEdit',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addEditDoctorModal').modal('hide');
                    reloadDoctorList();
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




function reloadDoctorList() {
    $.ajax({
        url: '/Doctor/List',
        type: 'GET',
        success: function (data) {
            $('#doctor').html(data);
        },
        error: function (xhr, status, error) {
            console.error("Error reloading doctor list:", error);
        }
    });
}
