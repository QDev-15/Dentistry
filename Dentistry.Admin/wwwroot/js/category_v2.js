$(document).ready(function () {
    let categoryCount = 0;

    $("#saveCategory").click(function () {
        let categoryName = $("#newCategoryName").val();
        let categoryType = $("#categoryType").val();
        let categoryPosition = $("#categoryPosition").val();
        let categoryAvatar = $("#categoryAvatar").val();
        let parentCategoryId = $("#parentCategory").val();

        if (categoryName.trim() !== "") {
            categoryCount++;
            let categoryId = `category${categoryCount}`;
            let newCategory = createCategoryElement(categoryId, categoryName, categoryType, categoryPosition, categoryAvatar);

            if (parentCategoryId) {
                let parentCollapse = $("#collapse" + parentCategoryId + " .nested-accordion");
                if (!parentCollapse.length) {
                    let parentBody = $("#collapse" + parentCategoryId + " .accordion-body");
                    parentBody.append('<div class="accordion nested-accordion"></div>');
                    parentCollapse = parentBody.find(".nested-accordion");
                }
                parentCollapse.append(newCategory);
            } else {
                $("#categoryAccordion").append(newCategory);
            }

            $("#parentCategory").append(`<option value="${categoryId}">${categoryName}</option>`);
            $("#addCategoryModal").modal("hide");
            $("#newCategoryName, #categoryType, #categoryPosition, #categoryAvatar").val("");
        }
    });

    function createCategoryElement(id, name, type, position, avatar) {
        return `<div class="accordion-item">
                    <h2 class="accordion-header" id="heading${id}">
                        <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${id}" aria-expanded="true" aria-controls="collapse${id}">
                            <div class="category-item">
                                <img src="${avatar}" class="category-avatar" alt="Avatar">
                                <span>${name} (Vị trí: ${position}, Loại: ${type})</span>
                            </div>
                        </button>
                    </h2>
                    <div id="collapse${id}" class="accordion-collapse collapse" aria-labelledby="heading${id}">
                        <div class="accordion-body d-flex justify-content-between">
                            <span>${name}</span>
                            <div>
                                <button class="btn btn-warning btn-sm edit-btn">Sửa</button>
                                <button class="btn btn-danger btn-sm delete-btn">Xóa</button>
                            </div>
                        </div>
                    </div>
                </div>`;
    }
    function updateElement(id, img, subName, name, position, type, sort) {
        $(".category-accordion-name-" + id).text(name);
        $(".category-accordion-subname-" + id).text(subName);
        $(".card-accordion-name-" + id).val(name);
        $(".card-accordion-positon-" + id).val(position);
        $(".card-accordion-type-" + id).val(type);
        $(".card-accordion-sort-" + id).val(sort);
        $(".category-avatar-" + id).attr('src', img);
    }

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
        var update = $("#item_Id").val() != "0";
        //debugger;
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.isSuccessed) {
                    $('#addCategoryModal').modal('hide');
                    if (update && response && response.data) {
                        updateElement(response.data.id, response.data.coverImage, response.data.subName, response.data.name, response.data.positionName, response.data.typeName, response.data.sort);
                    } else {
                        loadCategoryList();
                    }
                     // Hoặc cập nhật bảng
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
    $(document).on("click", ".delete-btn", function () {
        $(this).closest(".accordion-item").remove();
        let optionValue = $(this).closest(".accordion-item").attr("id");
        $("#parentCategory option[value='" + optionValue + "']").remove();
    });

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

