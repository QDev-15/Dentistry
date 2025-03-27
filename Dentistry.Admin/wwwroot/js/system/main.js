const defaultClassForSpinner = "sb-nav-fixed";// "layout-main-content";
const imgLoadingDefault = "/assets/img/loading/loading5.webp";
/*** =============== CHECK DOM loading and END LOADING =======================*/
const domStage = {
    running: false,
    method: null
}

/** +============================================================================= */

setTimezoneCookie();
document.addEventListener("DOMContentLoaded", function () {
    // Observer để kiểm tra khi DOM thay đổi
    let observer = new MutationObserver((mutations, observerInstance) => {
        if (domStage.method) {
            domStage.method();
        }
        domStage.running = false;
        domStage.method = null;
        mutations.forEach((mutation) => {
            mutation.addedNodes.forEach((node) => {
                if (node.tagName === 'IMG') {
                    checkImageValidity(node);
                    observer.observe(node, { attributes: true, attributeFilter: ['src'] });
                } else {
                    // Nếu không phải img, tìm trong tất cả các thẻ con
                    node.querySelectorAll && node.querySelectorAll("img").forEach(checkImageValidity);
                }
            });
        });
    });
    
    // Quan sát toàn bộ trang, kể cả các phần tử thêm vào sau
    observer.observe(document.body, { childList: true, subtree: true });
});
// Documents 
$(document).ready(function () {
    document.addEventListener('hidden.bs.modal', function (event) {
        const backdrops = document.querySelectorAll('.modal-backdrop');
        backdrops.forEach(backdrop => backdrop.remove());
    });
    popstateUpdate();
});


// Hàm lưu timezone vào cookie
function setTimezoneCookie() {
    const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    document.cookie = `timezone=${timezone}; path=/; max-age=31536000`; // Lưu 1 năm
}

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Handle back/forward navigation
window.addEventListener('popstate', function (event) {
    const url = window.location.pathname;
    $('#layoutSidenav_content main').load(url);
    popstateUpdate();
});

function popstateUpdate() {
    const url = window.location.pathname;
    $("#left-menu a.nav-link").removeClass("active");
    $("#left-menu a.nav-link").each(function () {
        var linkUrl = $(this).attr("href")?.toLowerCase();
        if (linkUrl == url || (linkUrl != url && linkUrl != '/' && url != '/')) {
            $(this).toggleClass("active", linkUrl && url.toLowerCase().indexOf(linkUrl) >= 0);
        }
    });
}

function openInfoModal() {
    // Open modal 
    const modal = new bootstrap.Modal(document.getElementById('infoModal'));
    if (bootstrap.Modal.getInstance(modal)) {
        bootstrap.Modal.getInstance(modal).dispose();
    }
    modal.show();
}
function resetFooterBtn() {
    //remove d-none
    $('#infoModal .modal-footer .btn-warning').removeClass('d-none');
}
function showInfo(message, title) {
    if (title && title.length > 0) {
        $('#infoModalLabel').text(title)
    } else {
        $('#infoModalLabel').text("Thông báo")
    }
    $('#infoModal #message-content').text(message);
    // set header
    $('.infoModal-header').css('background-color', 'rgb(13,202,240)');
    $('.infoModal-header').css('color', 'white');
    // set footer
    resetFooterBtn();
    $('#infoModal .modal-footer .btn-warning').addClass('d-none');


    // show modal
    openInfoModal();
}
function showSuccess(message, title) {
    if (title && title.length > 0) {
        $('#infoModalLabel').text(title)
    } else {
        $('#infoModalLabel').text("Thành công")
    }
    $('#infoModal #message-content').text(message);
    // set header
    $('.infoModal-header').css('background-color', 'Green');
    $('.infoModal-header').css('color', 'white');
    // set footer                                    
    resetFooterBtn();
    $('#infoModal .modal-footer .btn-warning').addClass('d-none');


    // show modal
    openInfoModal();
}
function showConfirm(message, title) {
    return new Promise(function (resolve, reject) {
        if (title && title.length > 0) {
            $('#infoModalLabel').text(title)
        } else {
            $('#infoModalLabel').text("Xác nhận")
        }
        $('#infoModal #message-content').text(message);
        // set header
        $('.infoModal-header').css('background-color', 'rgb(166,115,208)');
        $('.infoModal-header').css('color', 'white');
        // set footer                              
        resetFooterBtn();
        // show modal
        openInfoModal();
        $('#infoModal-confirm').off('click').on('click', function () {
            $('#infoModal').modal('hide'); // Ẩn modal
            resolve(true); // Trả về kết quả là true
        });

        // Lắng nghe sự kiện đóng modal (hủy)
        $('#infoModal').off('hidden.bs.modal').on('hidden.bs.modal', function () {
            resolve(false); // Trả về kết quả là false khi modal bị đóng
        });
    });
    
}
function showError(message, title) {
    if (title && title.length > 0) {
        $('#infoModalLabel').text(title)
    } else {
        $('#infoModalLabel').text("Lỗi")
    }
    $('#infoModal #message-content').text(message);
    // set header
    $('.infoModal-header').css('background-color', 'red');
    $('.infoModal-header').css('color', 'white');
    // set footer       
    resetFooterBtn();
    $('#infoModal .modal-footer .btn-warning').addClass('d-none');

    // show modal
    openInfoModal();
}
function closeAllModal() {
    $('.modal.show').each(function () {
        var modalInstance = bootstrap.Modal.getInstance(this);
        if (modalInstance) {
            modalInstance.hide();
        }
    });
}



var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
$("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function () {
    if (this.href === path) {
        $(this).addClass("active");
    }
});

// Toggle the side navigation
$("#sidebarToggle").on("click", function (e) {
    e.preventDefault();
    $("body").toggleClass("sb-sidenav-toggled");
});

// spinner  =========================================================================
function showGlobalSpinner() {
    document.getElementById('global-spinner').style.display = 'flex';
}

function hideGlobalSpinner() {
    document.getElementById('global-spinner').style.display = 'none';
}

function showSpinnerFor(className) {
    className = className ?? defaultClassForSpinner;
    const elements = document.querySelectorAll('.' + className);
    const spinnerOverlay = document.createElement('div');
    spinnerOverlay.classList.add('spinner-overlay');

    const spinnerHTML = `
                <div class="spinner-border" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            `;
    spinnerOverlay.innerHTML = spinnerHTML;

    // Thêm spinner vào trong element
    elements.forEach(element => {
        element.classList.add('spinner-container');
        element.appendChild(spinnerOverlay);
    });
    
}
function hideSpinnerFor(className) {
    className = className || defaultClassForSpinner;
    // Tìm và xóa spinner overlay
    const elements = document.querySelectorAll('.' + className);
    elements.forEach(element => {
        const spinnerOverlay = element.querySelector('.spinner-overlay');
        if (spinnerOverlay) {
            spinnerOverlay.remove();
        }
        element.classList.remove('spinner-container');
    });

}
/// ======================= End Spinner =============================
function loadSlideList() {
    showSpinnerFor();
    $.ajax({
        url: '/Slide/List',
        type: 'GET',
        success: function (data) {
            $('#slide-setting').html(data);
            hideSpinnerFor();
        },
        error: function (xhr, status, error) {
            showError("Error reloading doctor list:", error);
            hideSpinnerFor();
        }
    });
}
function loadCategoryList() {
    showSpinnerFor();
    $.ajax({
        url: '/Category/List',
        type: 'GET',
        success: function (data) {
            $('#category-setting').html(data);
            hideSpinnerFor();
        },
        error: function (xhr, status, error) {
            showError("Error reloading doctor list:", error);
            hideSpinnerFor();
        }
    });
}
function loadBranchesList() {
    showSpinnerFor();
    $.ajax({
        url: '/Branches/List',
        type: 'GET',
        success: function (data) {
            $('#branches-setting').html(data);
            hideSpinnerFor();
        },
        error: function (xhr, status, error) {
            showError("Error reloading branches list:", error);
            hideSpinnerFor();
        }
    });
}
function loadDoctorList() {
    showSpinnerFor();
    return $.ajax({
        url: '/Doctor/List',
        type: 'GET',
        //success: function (data) {
        //    $('#doctor-setting').html(data);
        //    hideSpinnerFor();
        //},
        //error: function (xhr, status, error) {
        //    showError("Error reloading doctor list:", error);
        //    hideSpinnerFor();
        //}
    }).done(function (data) {
        $('#doctor-setting').html(data);
    }).fail(function (xhr, status, error) {
        showError("Error reloading doctor list:", error);
    }).always(function () {
        hideSpinnerFor();
    });
}
function loadSettingData() {
    showSpinnerFor();
    $.ajax({
        url: `/AppSetting/GetSetting`,
        type: 'GET',
        success: function (html) {
            $('#app-setting').html(html);
            hideSpinnerFor();
        },
        error: function () {
            showError('Tải lại cài đặt thất bại!');
            hideSpinnerFor();
        }
    });
}


function checkImageValidity(img) {
    let src = img.getAttribute("src");
    
    // Nếu ảnh không có src hoặc src rỗng -> Gán ảnh mặc định
    if (!src || src.trim() === "") {
        img.setAttribute("src", imgLoadingDefault);
        return;
    }

    // Kiểm tra ảnh có tồn tại không bằng cách tạo một đối tượng Image mới
    let newImg = new Image();
    newImg.src = src;

    newImg.onload = function () {
        // Ảnh hợp lệ, giữ nguyên
        //console.log(`✅ Ảnh tồn tại: ${src}`);
    };

    newImg.onerror = function () {
        // Ảnh lỗi, thay thế bằng ảnh mặc định
        //console.log(`❌ Ảnh lỗi: ${src} -> Chuyển sang ảnh mặc định`);
        img.setAttribute("src", imgLoadingDefault);
    };
}


// Tiny =========================================================================
function initTiny(editorId) {
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

                    fetch('/article-upload-image?id=' + id, {
                        method: 'POST',
                        body: formData
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data && data.path) {
                                selectedAllIds.add(data.id);
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
            xhr.open('POST', '/article-upload-image?id=' + id);

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
                selectedAllIds.add(json.id);
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

