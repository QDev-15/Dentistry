$(document).ready(function () {
    $('#loggerTable').DataTable({
        processing: true,
        serverSide: true,
        stateSave: true,
        order: [[1, 'desc']],
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
            },
            dataSrc: function (json) {
                console.log("Server Response:", json); // Debug response
                return json.data || [];
            }
        },
        columns: [
            {
                data: "body",
                title: "Body",
                className: "text-wrap text-primary fw-bold"
            },
            {
                data: "createdDate",
                title: "Date",
                className: "text-center",
                render: function (data) {
                    if (!data) return "";
                    return moment(data).format("DD/MM/YYYY HH:mm:ss");

                    //const date = new Date(data);
                    //return date.toLocaleString('vi-VN', {
                    //    day: '2-digit', month: '2-digit', year: 'numeric',
                    //    hour: '2-digit', minute: '2-digit', second: '2-digit'
                    //});
                }
            },
            {
                data: "id",
                className: "text-center text-muted",
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
            url: `/Logger/Detail/${id}`,
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





