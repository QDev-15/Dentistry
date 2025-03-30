
    let currentPage = 1;
    const pageSize = 10;
    function animateNumber(element, start, end, duration) {
        let range = end - start;
        let startTime = null;

        function step(timestamp) {
            if (!startTime) startTime = timestamp;
            let progress = Math.min((timestamp - startTime) / duration, 1);
            element.textContent = Math.floor(progress * range + start);
            if (progress < 1) {
                requestAnimationFrame(step);
            }
        }
        requestAnimationFrame(step);
    }
function loadVisitorLogs(pageIndex) {
    currentPage = pageIndex;
    $.ajax({
        url: "/Access/GetVisitorLogs",
        type: "GET",
        data: { pageIndex: pageIndex, pageSize: pageSize },
        dataType: "json",
        beforeSend: function () {
            $("#active-users-list").html('<tr><td colspan="2" class="text-center">Đang tải...</td></tr>');
        },
        success: function (data) {
            let totalVisitsElem = $("#total-visits");
            let onlineUsersElem = $("#online-users");

            animateNumber(totalVisitsElem[0], parseInt(totalVisitsElem.text()) || 0, data.totalRecords, 1000);
            animateNumber(onlineUsersElem[0], parseInt(onlineUsersElem.text()) || 0, data.onlineUsers, 1000);

            let tableBody = $("#active-users-list");
            tableBody.empty();
            $.each(data.items, function (index, user) {
                tableBody.append(`<tr><td>${user.ipAddress}</td><td>${user.visitTime}</td></tr>`);
            });

            renderPagination(data.pageIndex, data.pageCount);
        },
        error: function (xhr) {
            console.error("Lỗi khi tải dữ liệu:", xhr.responseText);
            $("#active-users-list").html('<tr><td colspan="2" class="text-center text-danger">Lỗi khi tải dữ liệu</td></tr>');
        }
    });
}

function renderPagination(pageIndex, pageCount) {
    let pagination = document.getElementById('pagination');
    pagination.innerHTML = '';
    let maxPagesToShow = 5;
    let startPage = Math.max(1, pageIndex - 2);
    let endPage = Math.min(pageCount, startPage + maxPagesToShow - 1);

    if (startPage > 1) {
        pagination.innerHTML += `<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="loadVisitorLogs(1)">1</a></li>`;
        if (startPage > 2) {
            pagination.innerHTML += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        let activeClass = i === pageIndex ? 'active' : '';
        pagination.innerHTML += `<li class="page-item ${activeClass}"><a class="page-link" href="javascript:void(0);" onclick="loadVisitorLogs(${i})">${i}</a></li>`;
    }

    if (endPage < pageCount) {
        if (endPage < pageCount - 1) {
            pagination.innerHTML += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
        pagination.innerHTML += `<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="loadVisitorLogs(${pageCount})">${pageCount}</a></li>`;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    loadVisitorLogs(currentPage);

    document.getElementById('track-visitors').addEventListener('change', function () {
        $.ajax({
            url: "/AppSetting/UpdateVisitor",
            type: "POST",
            data: { id: 1, value: this.checked },
            success: function (data) {
                console.log("Done");
            },
            error: function (xhr) {
                console.error("Lỗi khi tải dữ liệu:", xhr.responseText);
            }
        });
    });
});

document.getElementById('refresh-button').addEventListener('click', function () {
    loadVisitorLogs(currentPage);
});
