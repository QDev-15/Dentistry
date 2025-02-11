
// Khi tải trang, kiểm tra query string và đặt tab được chọn
$(document).ready(function () {
    listenerEvent();
    setActiveTab();
});

function listenerEvent() {
    // select tab setting
    const tabSettingTabMenu = document.getElementById("tabSettingMenu");
    tabSettingTabMenu.addEventListener("shown.bs.tab", function (event) {
        const targetTab = event.target; // Button của tab được chọn
        const tabContentId = targetTab.getAttribute("data-tab"); // ID của tab-pane
        // save tabId to link
        const url = new URL(window.location.href);
        url.searchParams.set('selectedTab', tabContentId);
        history.pushState({}, '', url); // Cập nhật URL mà không reload
        // load data tab-content
        loadContentSelectedTab(tabContentId);
    });
}
function loadContentSelectedTab(tabId) {
    switch (tabId) {
        case 'slide-setting':
            loadSlideList();
            break;
        case 'category-setting':
            loadCategoryList();
            break;
        case 'branches-setting':
            loadBranchesList();
            break;
        case 'doctor-setting':
            loadDoctorList();
            break;
        case 'app-setting':
            loadSettingData();
            break; 
        default:
            break;
    }
}
function setActiveTab() {
    const urlParams = new URLSearchParams(window.location.search);
    const tabContentId = urlParams.get('selectedTab') || "slide-setting";
    $('#tabSettingMenu .nav-link').removeClass('active');
    $(`#tabSettingMenu .nav-link[data-tab=${tabContentId}]`).addClass('active');

    $('#tabSettingContent .tab-pane').removeClass('active');
    $('#tabSettingContent .tab-pane').removeClass('show');

    $(`#tabSettingContent #${tabContentId}`).addClass('show');
    $(`#tabSettingContent #${tabContentId}`).addClass('active');
    loadContentSelectedTab(tabContentId);
}

