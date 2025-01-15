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

});