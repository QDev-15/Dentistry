
$(document).ready(function () {
    let tagsHidden = $("#tagsHidden");
    let tags = tagsHidden.val() ? tagsHidden.val().split(",") : [];
    let tagContainer = $("#tagContainer");
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
            let newTag = $(this).val().trim();
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
    $(document).on('click', '.add-btn, .edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Articles/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                domStage.method = init;
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
