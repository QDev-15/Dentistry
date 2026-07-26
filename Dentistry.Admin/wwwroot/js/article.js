
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

    // preview ảnh đại diện - tab tải ảnh lên
    $(document).on('change', '#item_avatarFile', function () {
        const file = this.files[0];
        const $previewContainer = $('#article-avatar-preview-container');
        const $imagePreview = $('#article-avatar-preview');
        if (file) {
            $('#item_avatarUrl').val('');
            $('.unsplash-thumb').removeClass('selected');
            const reader = new FileReader();
            reader.onload = function (e) {
                $imagePreview.attr('src', e.target.result);
                $previewContainer.show();
            };
            reader.readAsDataURL(file);
        }
        $('#avatarFile-error').text('');
    });

    // preview ảnh đại diện - tab dán link ảnh
    $(document).on('click', '#btn-preview-avatar-url', function () {
        const url = $('#avatarUrlInput').val().trim();
        if (!url) return;
        $('#item_avatarFile').val('');
        $('.unsplash-thumb').removeClass('selected');
        $('#item_avatarUrl').val(url);
        $('#article-avatar-preview').attr('src', url);
        $('#article-avatar-preview-container').show();
        $('#avatarFile-error').text('');
    });

    // tìm kiếm ảnh trên Unsplash
    $(document).on('click', '#btn-search-unsplash', function () {
        const query = $('#unsplashQuery').val().trim();
        if (!query) return;
        const $results = $('#unsplash-results');
        $results.html('<span>Đang tìm kiếm...</span>');
        $.ajax({
            url: '/Articles/SearchUnsplash',
            type: 'GET',
            data: { query: query },
            success: function (photos) {
                $results.empty();
                if (!photos || photos.length === 0) {
                    $results.html('<span>Không tìm thấy ảnh phù hợp.</span>');
                    return;
                }
                photos.forEach(function (photo) {
                    const $img = $('<img>')
                        .addClass('unsplash-thumb')
                        .attr('src', photo.thumb)
                        .attr('title', photo.description || '')
                        .attr('data-full', photo.regular);
                    $results.append($img);
                });
            },
            error: function () {
                $results.html('<span>Tìm kiếm thất bại, vui lòng thử lại.</span>');
            }
        });
    });

    // chọn 1 ảnh Unsplash làm ảnh đại diện
    $(document).on('click', '.unsplash-thumb', function () {
        $('.unsplash-thumb').removeClass('selected');
        $(this).addClass('selected');
        $('#item_avatarFile').val('');
        const fullUrl = $(this).data('full');
        $('#item_avatarUrl').val(fullUrl);
        $('#article-avatar-preview').attr('src', fullUrl);
        $('#article-avatar-preview-container').show();
        $('#avatarFile-error').text('');
    });

    // submit modal
    $(document).on('submit', '#addArticleForm', function (e) {
        e.preventDefault();
        const avatarFile = document.getElementById('item_avatarFile');
        const avatarUrl = $('#item_avatarUrl').val().trim();
        const hasExistingAvatar = $('#article-avatar-preview-container').is(':visible');
        if (avatarFile.files.length === 0 && !avatarUrl && !hasExistingAvatar) {
            $('#avatarFile-error').text('Vui lòng chọn ảnh đại diện cho bài viết.');
            return;
        }
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
                    $('#addEditArticleModal').modal('hide');
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
            { data: 'coverImage', name: 'CoverImage', orderable: false, render: function (data, type, row) {
                    if (type === 'display') {
                        const isImage = data?.startsWith('http') || data?.startsWith('/');
                        return isImage ? `<img src="${data}" style="width:50px; height:50px; object-fit:cover;" />` : '';
                    }
                    return data;
                }
            },
            { data: 'title', name: 'Title', orderable: true },
            { data: 'categoryName', name: 'Category', orderable: true },
            { data: 'displayType', name: 'DisplayType', orderable: false },
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
            initTable();
        },
        error: function () {
            showError('Tải lại danh sách bài viết thất bại!');
        }
    });
}
