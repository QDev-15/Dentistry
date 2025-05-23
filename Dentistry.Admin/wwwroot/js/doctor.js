﻿$(document).ready(function () {

    // Initialize DataTable
    // open model edit-add
    $(document).on('click', '.add-btn, .edit-btn', function () {
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Doctor/AddEdit/${id}`,
            type: 'GET',
            success: function (html) {
                $('#addEditDoctorModal .modal-content').html(html);
                $('#addEditDoctorModal').modal('show');
                initDoctorTiny("doctor_Description");
            },
            error: function () {
                showError('Thất bại, xin vui lòng thử lại.');
            }
        });
    });
    $(document).on('click', '.refresh-btn', function () {
        loadDoctorList();
    });
    // submit modal
    $(document).on('submit', '#addDoctorForm', function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        formData.delete("formFile");
        formData.delete("backgroundFile");
        showGlobalSpinner();
        $.ajax({
            url: '/Doctor/AddEdit',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                hideGlobalSpinner();
                if (response.isSuccessed) {
                    $('#addEditDoctorModal').modal('hide');
                    const fileInput = document.getElementById("item_formFile");
                    const fileBackgInput = document.getElementById("item_backgroundFile");
                    var data = response.data;

                    loadDoctorList().then(() => {
                        // Nếu có file ảnh, tiếp tục upload ảnh (Bước 2)
                        var avatar = fileInput.files.length > 0 ? fileInput.files[0] : null;
                        var background = fileBackgInput.files.length > 0 ? fileBackgInput.files[0] : null;
                        if (background != null || avatar != null) {
                            uploadDoctorImage(data.id, avatar, background);
                        }
                    });

                    
                    //loadDoctorList().then(() => {
                        
                    //});
                } else {
                    showInfo(response.message);
                }
            },
            error: function (err) {
                hideGlobalSpinner();
                showError('Lưu thất bại.');
            }
        });
    });
    $(document).on('click', '.doctor-delete-btn', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');

        showConfirm("Xóa bác sĩ: " + name, "Xác nhận").then(function (resp) {
            if (resp == true) {
                deleteDoctor(id);
            };
        }, function (err) {
            showError(err)
        });

    });
    
});
var slideTable = null;
function initDoctorTable() {
    slideTable = $('#doctorTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        pageLength: 10, // Mặc định hiển thị 5 dòng
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Tất cả"]] // Các tùy chọn số dòng hiển thị
    });
}
function uploadDoctorImage(doctorId, file, backg) {
    const formData = new FormData();
    formData.append("id", doctorId);
    formData.append("imageFile", file);
    formData.append("backgroundFile", backg);

    $.ajax({
        url: '/Doctor/UploadImage',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.isSuccessed) {
                var data = response.data;
                if (backg != null) {
                    $(".backg-doctor-" + data.id).css("opacity", 0).attr("src", data.background.path).on("load", function () {
                        $(this).fadeTo(500, 1); // Làm mờ dần trong 500ms
                    });
                }
                if (file != null) {
                    $(".avatar-doctor-" + data.id).css("opacity", 0).attr("src", data.avatar.path).on("load", function () {
                        $(this).fadeTo(500, 1); // Làm mờ dần trong 500ms
                    });
                }

                /*$(".category-avatar-" + data.id).attr('src', data.coverImage);*/
                //showSuccess("Upload ảnh thành công");
            } else {
                showError("Upload ảnh thất bại");
            }
        },
        error: function (error) {
            showError("Lỗi khi upload ảnh");
        }
    });
}
function deleteDoctor(id) {
    showGlobalSpinner();
    $.ajax({
        url: `/Doctor/Delete/${id}`,
        type: 'GET',
        success: function (result) {
            hideGlobalSpinner();
            console.log("result delete: ", result);
            showSuccess("Xóa thành công!");
            loadDoctorList();
        },
        error: function (err) {
            hideGlobalSpinner();
            console.log("result error: ", err);
            showError("Xóa không thành công!")
        }
    });
}
function initDoctorTiny(editorId) {
    while (tinymce.get(editorId)) {
        tinymce.get(editorId).destroy();
    }
    tinymce.init({
        selector: '#' + editorId,
        plugins: [
            'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'media', 'link', 'lists',
            'searchreplace', 'table', 'visualblocks', 'wordcount', "code", "paste"
        ],
        forced_root_block: '',
        valid_children: "+body[style],+div[style],+span[style],+a[style]",
        content_css: "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css",
        inline_styles: true, // Giữ lại CSS nội tuyến
        valid_elements: '*[*]', // Giữ lại tất cả thẻ và thuộc tính
        extended_valid_elements: '*[*]',
        media_live_embeds: true, // Cho phép hiển thị trực tiếp video
        // Tắt các bộ lọc và kiểm tra tự động
        paste_enable_default_filters: false,   // Tắt bộ lọc mặc định khi dán nội dung
        paste_as_text: false,
        cleanup: false,
        verify_html: false,
        entity_encoding: "raw",
        remove_trailing_brs: false,

        // Không tự động chỉnh sửa URL
        convert_urls: false,
        image_advtab: true,
        relative_urls: false,
        remove_script_host: false,
        image_class_list: [ // Tùy chỉnh class cho ảnh
            { title: 'Responsive', value: 'img-fluid' },
            { title: 'Thumbnail', value: 'img-thumbnail' }
        ],
        paste_postprocess: function (plugin, args) {
            console.log('Pasted content:', args.node.innerHTML);
        },
        content_style: "img { margin: 10px;}",
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table | align lineheight | numlist bullist indent outdent | emoticons charmap | removeformat | code paste',
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







