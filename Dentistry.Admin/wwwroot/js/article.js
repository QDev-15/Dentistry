
$(document).ready(function () {
    let tagsHidden = $("#tagsHidden");
    let tags = tagsHidden.val() ? tagsHidden.val().split(",") : [];
    let tagContainer = $("#tagContainer");
    initTable();
    function init() {
        tagContainer = $("#tagContainer");
        tagsHidden = $("#tagsHidden");
        tags = tagsHidden.val() ? tagsHidden.val().split(",") : [];
        renderTags();
    }
    function renderTags() {
        tagContainer.html(""); // Xóa nội dung cũ

        tags.forEach((tag, index) => {
            tagContainer.append(`
                <span class="tag-item">
                    ${tag} 
                    <button type="button" class="tag-remove" data-index="${index}">&times;</button>
                </span>
            `);
        });

        tagContainer.append('<input type="text" class="tag-input" id="tagInput" placeholder="Nhập tag và nhấn Enter">');
        $("#tagsHidden").val(tags.join(",")); // Cập nhật giá trị input ẩn
    }

    // Xử lý khi nhấn Enter để thêm tag
    $(document).on("keypress", "#tagInput", function (e) {
        if (e.which === 13 || e.which === 44) {
            e.preventDefault();
            let newTag = $("#tagInput").val().trim();
            if (newTag && !tags.includes(newTag)) {
                tags.push(newTag);
                $(this).val(""); // Xóa input sau khi thêm
                renderTags();
                $("#tagInput").focus();
            }
        }
    });

    // Xử lý khi nhấn nút xóa tag
    $(document).on("click", ".tag-remove", function () {
        let index = $(this).data("index");
        tags.splice(index, 1);
        renderTags();
    });

    // Open modal add-edit
    $(document).off('click', '.add-btn, .edit-btn').on('click', '.add-btn, .edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Articles/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                domStage.method = init;
                $('#addEditArticleModal .modal-content').html(html);
                $('#addEditArticleModal').modal('show');
                selectedAllIds = new Set();
                initTiny("Item_Description");
                
            },
            error: function () {
                showError('Tải bài biết thất bại.');
            }
        });
    });
    $(document).on('change', '#show-active', function () {
        refreshData();
    });

    // submit modal
    $('#addEditArticleModal').on('submit', 'form', function (e) {
        e.preventDefault();
        $('#Item_ImageIds').val(Array.from(selectedAllIds).join(","));
        const formData = new FormData(this);
        showGlobalSpinner();
        $.ajax({
            url: 'Articles/AddEdit',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addEditArticleModal').modal('hide');
                    loadArticle();
                    showSuccess("Thành công");
                    hideGlobalSpinner();
                } else {
                    window.alert(response.message);
                    hideGlobalSpinner();
                }
            },
            error: function () {
                showError('Lưu bài biết thất bại.');
                hideGlobalSpinner();
            }
        });
    });

    
});
var selectedAllIds = new Set();



function deleteArt(id) {
    showGlobalSpinner();
    $.ajax({
        url: `/Articles/Delete/${id}`,
        type: 'Delete',
        success: function (result) {
            console.log("result delete: ", result);
            loadArticle();
            showSuccess("Deactive bài viết thành công!")
            hideGlobalSpinner();
        },
        error: function (err) {
            console.log("result error: ", err);
            hideGlobalSpinner();
        }
    });
}
function confirmDeleteArticle(title, id) {
    showConfirm("Bạn có chắc chắn muốn ẩn bài viết: " + title, "Ẩn bài viết: " + title).then(function (resp) {
        if (resp == true) {
            deleteArt(id);
        };
    }, function (err) {
        alter(err)                        
    });
}
let tableArticle = null;
function refreshData() {
    if (!tableArticle || !tableArticle.ajax) return;
    tableArticle.ajax.reload(null, false);
}
function initTable() {
    let searchDelayTimer;

    tableArticle = $('#articleTable').DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        order: [[0, 'asc']],
        ajax: {
            url: '/Articles/GetDataTable',
            type: 'GET',
            data: function (d) {
                d.isActive = $("#show-active").prop('checked');
                d.searchValue = d.search.value || ""; // 🔥 Đảm bảo luôn gửi chuỗi rỗng nếu không có giá trị
                d.sortColumn = d.columns[d.order[0].column].data;  // Cột đang được sort
                d.sortDirection = d.order[0].dir; // Hướng sort (asc / desc)
            }
        },
        columns: [
            { data: 'title', name: 'Title', orderable: true }, // ✅ Cho phép sắp xếp
            { data: 'categoryName', name: 'Category', orderable: true }, // ✅ Cho phép sắp xếp
            { data: 'type', name: 'Type', orderable: true },
            { data: 'createdByName', name: 'CreatedBy', orderable: false },
            {
                data: 'createdDate',
                type: 'date',
                orderable: true, // ✅ Cho phép sắp xếp
                render: function (data) {
                    if (!data) return "";
                    const date = new Date(data);
                    const day = date.getDate().toString().padStart(2, '0');
                    const month = (date.getMonth() + 1).toString().padStart(2, '0');
                    const year = date.getFullYear();
                    const hours = date.getHours().toString().padStart(2, '0');
                    const minutes = date.getMinutes().toString().padStart(2, '0');
                    return `${day}/${month}/${year} ${hours}:${minutes}`; // Hiển thị theo HH:mm dd/MM/yyyy
                }
            },
            {
                data: 'isActive',
                orderable: true, // ✅ Cho phép sắp xếp
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Active</span>'
                        : '<span class="badge bg-danger">Inactive</span>';
                }
            },
            {
                data: 'id',
                orderable: false, // ❌ Không cho phép sắp xếp cột Actions
                render: function (data, type, row) {
                    var result = `<div class="container d-flex gap-2 justify-content-center">`;
                        result += `<button class="btn btn-sm btn-warning edit-btn px-1" data-bs-toggle="modal" data-bs-target="#addEditArticleModal" data-id="${data}">Edit</button>`;
                    if (row.isActive) {
                        result += `<button class="btn btn-sm btn-danger delete-btn px-1" onclick="confirmDeleteArticle('${row.title}', ${data})">Deactive</button>`;
                    }
                    result += `</div>`;
                    return result;
                }
            }
        ]
    });
    // 🔥 Debounce khi người dùng nhập vào ô tìm kiếm
    $('#articleTable_filter input').off().on('keyup', function () {
        clearTimeout(searchDelayTimer); // Xóa timer cũ
        let searchTerm = this.value;

        searchDelayTimer = setTimeout(function () {
            table.search(searchTerm).draw();
        }, 500); // 🔥 Delay 0.5 giây
    });
}
var loadArticle = function() {
    $.ajax({
        url: `/Articles/list`,
        type: 'GET',
        success: function (html) {
            $('#article-list').html(html);
        },
        error: function () {
            showError('Tải lại danh sách bài viết thất bại!');
        }
    });
}
