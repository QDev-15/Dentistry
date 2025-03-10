$(document).ready(function () {
    const MODAL_KEY = "lastModalTime";
    const ONE_HOUR = 60 * 60 * 1000;

    function showModal() {
        const modalButton = document.getElementById("openModal");
        modalButton.click(); // Kích hoạt nút mở modal ẩn
        localStorage.setItem(MODAL_KEY, Date.now());
        $("#openFeedbackModal").hide();
    }
    $("#close_button_modalOnloadDefaul").on("click", function () {
        $("#openFeedbackModal").show();
    });

    const lastTime = localStorage.getItem(MODAL_KEY);
    const currentTime = Date.now();

    if (!lastTime || currentTime - lastTime > ONE_HOUR) {
        showModal();
    }

    // Hiển thị modal
    $("#openFeedbackModal").on("click", function () {
        showModal();
    });


    // Back to Top =================================================================================
    // Hiển thị nút khi cuộn xuống 200px
    window.onscroll = function () {
        const backToTopButton = document.getElementById("backToTop");
        if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
            backToTopButton.classList.remove("e-hidden");
            $(".zalo-chat").css("bottom", "75px");
            $(".call-phone").css("bottom", "135px");
        } else {
            backToTopButton.classList.add("e-hidden");
            $(".zalo-chat").css("bottom", "20px");
            $(".call-phone").css("bottom", "75px");
        }
    };

    // Khi click, cuộn về đầu trang
    document.getElementById("backToTop").addEventListener("click", function () {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });
});

//Resize Screen
function getBootstrapBreakpoint(width) {
    width = width | window.innerWidth;
    if (width < 576) return "xs";      // Extra small
    if (width < 768) return "sm";      // Small
    if (width < 992) return "md";      // Medium
    if (width < 1200) return "lg";     // Large
    if (width < 1400) return "xl";     // Extra large
    return "xxl";                      // Extra extra large
}

function showScreenSize() {
    const width = window.innerWidth;
    const breakpoint = getBootstrapBreakpoint(width);
    console.log(`Width: ${width}px - Bootstrap: ${breakpoint}`);
}

// Gọi lần đầu tiên khi trang load
window.addEventListener("load", showScreenSize);
window.addEventListener("resize", showScreenSize);

//End Resize Screen


// Cookie
             function setCookie(name, value, days) {
    let expires = "";
    if (days) {
        let date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + "; path=/" + expires;
}

function getCookie(name) {
    let nameEQ = name + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i].trim();
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length);
    }
    return null;
}

// End Cookie

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