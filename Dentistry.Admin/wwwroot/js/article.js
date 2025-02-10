$(document).ready(function () {
    let tags = [];

    getTags();
    function getTags() {
        // Load tags từ input ẩn nếu có
        let existingTags = $('#TagsJson').val();
        if (existingTags) {
            tags = JSON.parse(existingTags);
        }
    }
    function renderTags() {
        $('#tagList').empty();
        tags.forEach((tag, index) => {
            $('#tagList').append(`
                <span class="tag-item">
                    ${tag.Name} <span class="remove-tag" data-tagid="${tag.Id}" data-index="${index}">×</span>
                </span>
            `);
        });

        // Cập nhật input ẩn
        $('#TagsJson').val(JSON.stringify(tags));
    }

    $(document).on('click', '#addTagBtn', function () {
        getTags();
        let tagInput = $('#tagInput').val().trim();
        if (tagInput) {
            tags.push({ id: 0, name: tagInput }); // Id = 0, server sẽ tự gán ID khi lưu
            renderTags();
            $('#tagInput').val('');
        }
    });

    $(document).on('keypress', '#tagInput', function (e) {
        if (e.which === 13) { // Nhấn Enter
            e.preventDefault();
            $('#addTagBtn').click();
        }
    });

    $(document).on('click', '.remove-tag', function () {
        getTags();
        let index = $(this).data('index');
        tags.splice(index, 1);
        renderTags();
    });

    renderTags(); // Render danh sách ban đầu


    // Open modal add-edit
    $(document).on('click', '.add-btn, .edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Articles/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditArticleModal .modal-content').html(html);
                $('#addEditArticleModal').modal('show');
                initTiny("Item_Description");
            },
            error: function () {
                showError('Tải bài biết thất bại.');
            }
        });
    });

  
    $(document).on('click', '#addEditArticleModal .action-close', function () {
        const id = $('#Item_Id').val();
        var isDraft = $('#Item_IsDraft').val()?.toLowerCase() == 'true';
        if (isDraft) {
            deleteArt(id);
        }
    });

    // submit modal
    $('#addEditArticleModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        $.ajax({
            url: 'Articles/AddEdit',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#addEditArticleModal').modal('hide');
                    loadArticle();
                } else {
                    window.alert(response.message);
                }
            },
            error: function () {
                showError('Lưu bài biết thất bại.');
            }
        });
    });

    
});




function deleteArt(id) {
    $.ajax({
        url: `/Articles/Delete/${id}`,
        type: 'Delete',
        success: function (result) {
            console.log("result delete: ", result);
            loadArticle();
        },
        error: function (err) {
            console.log("result error: ", err);
        }
    });
}
function confirmDeleteArticle(title, id) {
    showConfirm("Bạn có chắc chắn muốn xóa bài viết: " + title, "Xóa: " + title).then(function (resp) {
        if (resp == true) {
            deleteArt(id);
        };
    }, function (err) {
        alter(err)                        
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
