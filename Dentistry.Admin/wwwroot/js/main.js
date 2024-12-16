// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Handle back/forward navigation
window.onpopstate = function () {
    const url = window.location.pathname;
    $('#layoutSidenav_content main').load(url);
};

var runAction = null;

function openInfoModal() {
    // Open modal 
    const modal = new bootstrap.Modal(document.getElementById('infoModal'));
    modal.show();
}
function resetFooterBtn() {
    //remove d-none
    $('#infoModal .modal-footer .btn-danger').removeClass('d-none');
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
    $('#infoModal .modal-footer .btn-danger').addClass('d-none');
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
    $('#infoModal .modal-footer .btn-danger').addClass('d-none');
    $('#infoModal .modal-footer .btn-warning').addClass('d-none');


    // show modal
    openInfoModal();
}
function showConfirm(message, title) {
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
    $('#infoModal .modal-footer .btn-danger').addClass('d-none');
    // show modal
    openInfoModal();
}
function showConfirmDelete(returnAction, value) {
    $('#infoModalLabel').text("Xác nhận")
    $('#infoModal #message-content').text("Bạn có chắc chắn muốn xóa?");
    runAction = returnAction;
    // set value
    document.getElementById('confirmModalDeleteButton').setAttribute('data-id', value);
    // set header
    $('.infoModal-header').css('background-color', 'rgb(166,115,208)');
    $('.infoModal-header').css('color', 'white');
    // set footer                  
    resetFooterBtn();
    $('#infoModal .modal-footer .btn-warning').addClass('d-none');
    // show modal
    openInfoModal();
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
    $('#infoModal .modal-footer .btn-danger').addClass('d-none');
    $('#infoModal .modal-footer .btn-warning').addClass('d-none');

    // show modal
    openInfoModal();
}
$('#confirmModalDeleteButton').off('click').on('click', function () {
    if (runAction) {
        runAction.then(function (resp) {
            if (resp) {
                console.log(resp);
            }
            runAction = null;
        });
    }
});
function closeModal() {
    $('.modal.show').each(function () {
        var modalInstance = bootstrap.Modal.getInstance(this);
        if (modalInstance) {
            modalInstance.hide();
        }
    });
}
const confirmDeleteButton = document.getElementById('confirmModalDeleteButton');

// Xử lý khi nhấn nút "Delete" trong modal
confirmDeleteButton.addEventListener('click', function () {
    
});


