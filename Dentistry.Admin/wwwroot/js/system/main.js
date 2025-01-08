// Documents 
$(document).ready(function () {
    document.getElementById('infoModal').addEventListener('hidden.bs.modal', function () {
        const backdrops = document.querySelectorAll('.modal-backdrop');
        backdrops.forEach(backdrop => backdrop.remove());
    });
});


// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Handle back/forward navigation
window.onpopstate = function () {
    const url = window.location.pathname;
    $('#layoutSidenav_content main').load(url);
};


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
function showSpinner() {
    document.getElementById('global-spinner').style.display = 'flex';
}

function hideSpinner() {
    document.getElementById('global-spinner').style.display = 'none';
}

function showSpinnerFor(className) {
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


// Tiny =========================================================================
function initTiny(editorId) {
    while (tinymce.get(editorId)) {
        tinymce.get(editorId).destroy();
    }
    tinymce.init({
        selector: '#' + editorId,
        plugins: [
            'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists',
            'searchreplace', 'table', 'visualblocks', 'wordcount'
        ],
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image table | align lineheight | numlist bullist indent outdent | emoticons charmap | removeformat',
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

