﻿const defaultClassForSpinner = "layout-main-content";

/*** =============== CHECK DOM loading and END LOADING =======================*/
const domStage = {
    running: false,
    method: null
}

/** +============================================================================= */


document.addEventListener("DOMContentLoaded", function () {
    // Observer để kiểm tra khi DOM thay đổi
    let observer = new MutationObserver((mutations, observerInstance) => {
        if (domStage.method) {
            domStage.method();
        }
        domStage.running = false;
        domStage.method = null;
    });
    // Quan sát toàn bộ trang, kể cả các phần tử thêm vào sau
    observer.observe(document.body, { childList: true, subtree: true });

    var userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    fetch('/Home/SetUserTimeZone', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ timeZone: userTimeZone })
    });
});
// Documents 
$(document).ready(function () {
    document.addEventListener('hidden.bs.modal', function (event) {
        const backdrops = document.querySelectorAll('.modal-backdrop');
        backdrops.forEach(backdrop => backdrop.remove());
    });
    popstateUpdate();
});



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

//// mở Spinner khi  AJAX request
//$(document).ajaxStart(function () {
//    console.log("start spinner");
//    $("#global-spinner").show();
//});

//// Ẩn Spinner khi tất cả AJAX request hoàn tất
//$(document).ajaxStop(function () {
//    console.log("end spinner");
//    $("#global-spinner").hide();
//});
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
    $.ajax({
        url: '/Doctor/List',
        type: 'GET',
        success: function (data) {
            $('#doctor-setting').html(data);
            hideSpinnerFor();
        },
        error: function (xhr, status, error) {
            showError("Error reloading doctor list:", error);
            hideSpinnerFor();
        }
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




// Tiny =========================================================================
function initTiny(editorId) {
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

