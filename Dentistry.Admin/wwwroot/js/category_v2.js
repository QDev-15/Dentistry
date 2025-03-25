$(document).ready(function () {
    let categoryCount = 0;


    function createElement(id, img, levelName, levelValue, subName, name, position, type, sort) {
        var dmCap = "cấp 2";
        if (levelName === "Level2") {
            dmCap = "cấp 3";
        }
        var element = `<div id="accordion-item-${id}" class="accordion-item ${levelName}">
            <h2 class="accordion-header" id="headingcategory${id}">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapsecategory${id}" aria-expanded="false" aria-controls="collapsecategory${id}">
                    <div class="category-item">
                        <img src="${img}" class="category-avatar category-avatar-${id}" alt="Avatar">
                        <span>&ensp;<b class="category-accordion-name-${id}">${name}</b> &emsp; <span class="category-accordion-subname-${id}">${subName}</span></span>    
                    </div>
                </button>
            </h2 >
            <div id="collapsecategory${id}" class="accordion-collapse collapse" aria - labelledby="headingcategory${id}">
                <div class="accordion-body d-flex flex-column">
                    <div class="d-flex justify-content-between mb-2">
                        <div></div>
                        <div>
                            <button class="btn btn-warning btn-sm edit-category-btn" data-id="${id}" data-level="${levelValue}">Sửa</button>
                            <button class="btn btn-danger btn-sm delete-category-btn" data-id="${id}" data-name="${name}">Xóa</button>
                        </div>
                    </div>
                    <div class="card-accordion-${id} card p-3 mb-2">
                        <div class="mb-3">
                            <label class="form-label">Tên danh mục</label>
                            <input type="text" class="form-control card-accordion-name-${id}" value="${name}" readonly>
                        </div>
                        <div class="row">`;
            if (levelName === "Level1") {
                element += `<div class="col-md-4 mb-3">
                                <label class="form-label">Vị trí</label>
                                <input type="text" class="form-control  card-accordion-position-${id}" value="${position}" readonly>
                            </div>`;
            }
                element += `<div class="col-md-4 mb-3">
                                <label class="form-label">Loại danh mục</label>
                                <input type="text" class="form-control card-accordion-type-${id}" value="${type}" readonly>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">Thứ tự</label>
                                <input type="text" class="form-control card-accordion-sort-${id}" value="${sort}" readonly>
                            </div>
                        </div>
                    </div>`
        if (levelName !== "Level3") {
            element +=`<div class="d-flex justify-content-between mb-2" >
                            <div></div>
                            <div>
                                <button class="btn btn-primary add-category-btn" data-id="0" data-level="${parseInt(levelValue, 10) + 1}" data-parent="${id}">Thêm danh mục ${dmCap}</button>
                            </div>
                        </div>`
        }
        element += `<div id="nested-accordion-${id}" class="accordion nested-accordion" >
                    </div>
                </div>
            </div>
        </div>`
        return element;
    }
    function updateElement(id, img, subName, name, position, type, sort) {
        $(".category-accordion-name-" + id).text(name);
        $(".category-accordion-subname-" + id).text(subName);
        $(".card-accordion-name-" + id).val(name);
        $(".card-accordion-positon-" + id).val(position);
        $(".card-accordion-type-" + id).val(type);
        $(".card-accordion-sort-" + id).val(sort);
        $(".category-avatar-" + id).attr('src', img);
        checkImageValidity($(".category-avatar-" + id)[0]);
    }
    $(document).on('click', '#btnRefreshCategory', function () {
        loadCategoryList(); // Hàm này gọi lại API để cập nhật danh sách
    });
    $(document).on('click', '.delete-category-btn', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');

        showConfirm("Xóa danh mục: " + name, "Xác nhận").then(function (resp) {
            if (resp == true) {
                deleteCategory(id);
            };
        }, function (err) {
            alter(err)
        });

    });
    $(document).on("click", ".add-category-btn, .edit-category-btn", function (e) {
        e.preventDefault();
        const id = $(this).data('id') || 0; // Nếu không có ID, thì tạo mới
        const level = $(this).data('level'); // Nếu không có ID, thì tạo mới
        const parent = $(this).data('parent') || 0; // Nếu không có ID, thì tạo mới

        $.ajax({
            url: `/Category/AddEdit/${id}?level=${level}&parentId=${parent}`,
            type: 'GET',
            success: function (data) {
                $('#addCategoryModal .modal-content').html(data);
                $('#addCategoryModal').modal('show');
                initCategoryTiny("Cat_Item_Description");
            },
            error: function (err) {
                showError('Failed to load data');
            }
        });
    });
    // submit modal
    $(document).on('submit', '#addEditCategoryForm', function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        var id = $("#item_Id").val();
        // Bước 1: thêm mới/cập nhập text không bao gồm file
        formData.delete("item.ImageFile");
        var update = id != "0";
        showGlobalSpinner();
        //debugger;
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                hideGlobalSpinner();
                if (response.isSuccessed) {
                    $('#addCategoryModal').modal('hide');
                    var data = response.data;
                    if (update && data) {
                        updateElement(data.id, data.coverImage, data.subName, data.name, data.positionName, data.typeName, data.sort);
                    } else {
                        var newCategoryElement = createElement(data.id, data.coverImage, data.levelName, data.levelValue, data.subName, data.name, data.position, data.type, data.sort);
                        if (data.levelName === "Level1") {
                            $("#categoryAccordion").append(newCategoryElement);
                        } else {
                            $("#nested-accordion-" + data.parentId).append(newCategoryElement);
                        }
                        //loadCategoryList();
                    }
                    // Nếu có file ảnh, tiếp tục upload ảnh (Bước 2)
                    const fileInput = document.getElementById("item_ImageFile");
                    if (fileInput.files.length > 0) {
                        uploadCategoryImage(data.id, fileInput.files[0]);
                    } 
                } else {
                    if (response.data) {
                        $('#addCategoryModal .modal-content').html(html);
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
    // Hàm upload ảnh riêng biệt sau khi đã lưu slide thành công
    function uploadCategoryImage(categoryId, file) {
        const formData = new FormData();
        formData.append("id", categoryId);
        formData.append("imageFile", file);

        $.ajax({
            url: '/Category/UploadImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    var data = response.data;
                    $(".category-avatar-" + data.id).css("opacity", 0).attr("src", data.coverImage).on("load", function () {
                        $(this).fadeTo(500, 1); // Làm mờ dần trong 500ms
                    });
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
    function deleteCategory(id) {
        showGlobalSpinner();
        $.ajax({
            url: `/Category/Delete/${id}`,
            type: 'GET',
            success: function (result) {
                hideGlobalSpinner();
                console.log("result delete: ", result);
                $("#accordion-item-" + id).remove();
                /*loadCategoryList();*/
            },
            error: function (err) {
                hideGlobalSpinner();
                console.log("result error: ", err);
            }
        });
    }
});

function initCategoryTiny(editorId) {
    while (tinymce.get(editorId)) {
        tinymce.get(editorId).destroy();
    }
    tinymce.init({
        selector: '#' + editorId,
        plugins: [
            'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'media', 'link', 'lists',
            'searchreplace', 'table', 'visualblocks', 'wordcount', "code", "paste"
        ],
        paste_enable_default_filters: false, // Tắt bộ lọc mặc định khi dán nội dung
        valid_elements: '*[*]', // Giữ lại tất cả thẻ và thuộc tính
        media_live_embeds: true, // Cho phép hiển thị trực tiếp video
        extended_valid_elements: "h2,h3,p,strong,em,ul,ol,li,img[class|src|alt|width|height|style],a[class,href|style|target]",
        image_advtab: true,
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

