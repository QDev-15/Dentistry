$(document).ready(function () {
       

    // Open modal add-edit
    $(document).on('click', '.add-edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Articles/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditArticleModal .modal-content').html(html);
                $('#addEditArticleModal').modal('show');
                initTiny();
            },
            error: function () {
                showError('Tải bài biết thất bại.');
            }
        });
    });

   

    // delete article when close(draft), click button delete
    $(document).on('click', '.delete-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        showConfirmDelete(deleteArt, id);
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
            url: $(this).attr('action'),
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
var deleteArt = function(id) {
    $.ajax({
        url: `/Articles/Delete/${id}`,
        type: 'Delete',
        success: function (result) {
            console.log("result delete: ", result);
            closeModal();
            loadArticle();
        },
        error: function (err) {
            console.log("result error: ", err);
        }
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
var initTiny = function () {
    var editorId = 'Item_Description';
    if (tinymce.get(editorId)) {
        tinymce.get(editorId).destroy();
    }
    tinymce.init({
        selector: '#Item_Description',
        plugins: [
            'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists', 'media',
            'searchreplace', 'table', 'visualblocks', 'wordcount', 'checklist', 'mediaembed', 'casechange',
            'export', 'formatpainter', 'pageembed', 'a11ychecker', 'tinymcespellchecker', 'permanentpen',
            'powerpaste', 'advtable', 'advcode', 'editimage', 'advtemplate', 'mentions',
            'tableofcontents', 'footnotes', 'mergetags', 'autocorrect', 'typography', 'inlinecss'
        ],
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
        height: 500,
        automatic_uploads: true,
        // File picker configuration (Choose file manually)
        file_picker_callback: function (callback, value, meta) {
            if (meta.filetype === 'image') {
                var input = document.createElement('input');
                input.setAttribute('type', 'file');
                input.setAttribute('accept', 'image/*');

                input.onchange = function () {
                    var file = this.files[0];
                    var formData = new FormData();
                    formData.append('file', file);
                    var id = $('#Item_Id').val();

                    fetch('/article-upload-image?id=' + id, {
                        method: 'POST',
                        body: formData
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data && data.path) {
                                callback(data.path, { alt: file.name }); // Provide URL to TinyMCE
                            } else {
                                console.error('Upload failed: No URL returned');
                            }
                        })
                        .catch(error => console.error('Upload error:', error));
                };

                input.click();
            }
        },

        // Drag-and-drop configuration
        images_upload_handler: function (blobInfo, success, failure) {
            var formData = new FormData();
            formData.append('file', blobInfo.blob(), blobInfo.filename());
            var id = $('#Item_Id').val();
            const baseUrl = window.location.origin;
            fetch('/article-upload-image?id=' + id, {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data && data.path) {
                        const imageUrl = `${baseUrl}${data.path}`;
                        success(imageUrl); // Pass the uploaded image URL to TinyMCE
                    } else {
                        failure('Invalid server response');
                    }
                })
                .catch(error => {
                    console.error('Image upload error:', error);
                    failure('Image upload failed');
                });
        },

        // Configuration for TinyMCE features
        link_assume_external_targets: true,
        link_default_protocol: 'http',
        mergetags_list: [
            { value: 'First.Name', title: 'First Name' },
            { value: 'Email', title: 'Email' },
        ],
    });
}