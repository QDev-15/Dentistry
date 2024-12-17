
// Khi nhấn vào tab, lưu trạng thái vào query string
$(document).on('click', '#tabMenu .nav-link', function (e) {
    e.preventDefault();
    const tabId = $(this).data('tab');
    const url = new URL(window.location.href);
    url.searchParams.set('selectedTab', tabId);
    history.pushState({}, '', url); // Cập nhật URL mà không reload

    // Đặt tab được chọn
    //setActiveTab(tabId);
});

// Khi tải trang, kiểm tra query string và đặt tab được chọn
$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const selectedTab = urlParams.get('selectedTab');
    if (!selectedTab) return;
    setActiveTab(selectedTab);
});

function setActiveTab(tabId) {
    $('#tabMenu .nav-link').removeClass('active');
    $(`#tabMenu .nav-link[data-tab=${tabId}]`).addClass('active');

    $('#tabContent .tab-pane').removeClass('active');
    $('#tabContent .tab-pane').removeClass('show');

    $(`#tabContent #${tabId}`).addClass('show');
    $(`#tabContent #${tabId}`).addClass('active');
}