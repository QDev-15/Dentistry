$(document).ready(function () {
    // Initialize DataTable
    $('#categoryTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5,
        order: [[3, 'asc']],
    });
    $('#subCategoryTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 5
    });
    // handle Edit - Add button click
    $(document).on('click', '.category-edit-btn, .category-add-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        const parent = $(this).data('parent'); // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Category/AddEdit/${id}?parent=${parent}`,
            type: 'GET',
            success: function (data) {
                $('#addEditCategoryModal .modal-content').html(data);
                $('#addEditCategoryModal').modal('show');
                initCategoryTiny("Cat_Item_Description");
            },
            error: function (err) {
                showError('Failed to load data');
            }
        });
    });
    // submit modal
    $('#addEditCategoryModal').on('submit', 'form', function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        //debugger;
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addEditCategoryModal').modal('hide');
                    loadCategoryList(); // Hoặc cập nhật bảng
                } else {
                    if (response.data) {
                        $('#addEditCategoryModal .modal-content').html(html);
                    } else {
                        showError(response.message);
                    }
                }

            },
            error: function () {
                showError('Failed to save changes');
            }
        });
    });

    //const confirmDeleteModal = document.getElementById('confirmDeleteModal');
    //confirmDeleteModal.addEventListener('show.bs.modal', function (event) {
    //    // Lấy nút đã kích hoạt modal
    //    const button = event.relatedTarget;
    //    // Lấy giá trị ID từ data-id của nút
    //    const id = button.getAttribute('data-id');
    //    // Gán ID vào input ẩn trong modal
    //    const deleteIdInput = document.getElementById('deleteId');
    //    deleteIdInput.value = id;
    //});

});


function deleteCategory(id) {
    $.ajax({
        url: `/Category/Delete/${id}`,
        type: 'Delete',
        success: function (result) {
            console.log("result delete: ", result);
            loadCategoryList();
        },
        error: function (err) {
            console.log("result error: ", err);
        }
    });
}
function confirmDeleteCategory(title, id) {
    showConfirm("Bạn có chắc chắn xóa danh mục: " + title, "Xóa: " + title).then(function (resp) {
        if (resp == true) {
            deleteCategory(id);
        };
    }, function (err) {
        showError(err)
    });
}

function initCategoryTiny(editorId) {
    while (tinymce.get(editorId)) {
        tinymce.get(editorId).destroy();
    }
    tinymce.init({
        selector: '#' + editorId,
        plugins: [
            'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'media', 'link', 'lists',
            'searchreplace', 'table', 'visualblocks', 'wordcount', "code"
        ],
        media_live_embeds: true, // Cho phép hiển thị trực tiếp video
        extended_valid_elements: "h2,h3,p,strong,em,ul,ol,li,img[src|alt|style],a[href|style|target]",
        image_advtab: true,
        content_style: "img { margin: 10px;}",
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table | align lineheight | numlist bullist indent outdent | emoticons charmap | removeformat | code',
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

                    fetch('/upload-image', {
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
        images_upload_handler: (blobInfo, progress) => new Promise((resolve, reject) => {
            const xhr = new XMLHttpRequest();
            xhr.withCredentials = false;
            var id = $('#Item_Id').val();
            xhr.open('POST', '/upload-image');

            xhr.upload.onprogress = (e) => {
                progress(e.loaded / e.total * 100);
            };

            xhr.onload = () => {
                if (xhr.status === 403) {
                    reject({ message: 'HTTP Error: ' + xhr.status, remove: true });
                    return;
                }

                if (xhr.status < 200 || xhr.status >= 300) {
                    reject('HTTP Error: ' + xhr.status);
                    return;
                }

                const json = JSON.parse(xhr.responseText);

                if (!json || typeof json.path != 'string') {
                    reject('Invalid JSON: ' + xhr.responseText);
                    return;
                }

                resolve(json.path);
            };

            xhr.onerror = () => {
                reject('Image upload failed due to a XHR Transport error. Code: ' + xhr.status);
            };

            const formData = new FormData();
            formData.append('file', blobInfo.blob(), blobInfo.filename());

            xhr.send(formData);
        })
    });
}


