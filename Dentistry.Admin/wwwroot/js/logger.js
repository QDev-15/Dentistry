$(document).ready(function () {
    $('#loggerTable').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Logger/GetLogs',
            type: 'GET',
            data: function (d) {
                return {
                    PageIndex: (d.start / d.length) + 1, // Chuyển đổi start thành số trang
                    PageSize: d.length,
                    keySearch: d.search.value || "", // Truyền keySearch từ ô tìm kiếm của DataTables
                    sortColumn: d.order.length > 0 ? d.columns[d.order[0].column].data : "createdDate",
                    sortDirection: d.order.length > 0 ? d.order[0].dir : "desc"
                };
            }
        },
        columns: [
            { data: "body", title: "Body" },
            { data: "createdDate", title: "Date" },
            {
                data: "id",
                render: function (data) {
                    return `<button class="btn btn-sm btn-warning logger-view-btn" data-id="${data}">View</button>`;
                },
                orderable: false
            }
        ],
        pageLength: 10
    });

    $(document).on('click', '.logger-view-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Logger/View/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditLoggerModal .modal-content').html(html);
                $('#addEditLoggerModal').modal('show');
            },
            error: function (err) {
                showError('Failed to load data');
            }
        });
    });
});





