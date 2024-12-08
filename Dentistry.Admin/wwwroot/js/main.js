// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Handle back/forward navigation
window.onpopstate = function () {
    const url = window.location.pathname;
    $('#layoutSidenav_content main').load(url);
};

function loadData(url) {
    $('#layoutSidenav_content main').load(url, function (response, status, xhr) {
        if (status === "error") {
            $('#layoutSidenav_content main').html(`<p>Error loading content: ${xhr.statusText}</p>`);
        } else {
            history.pushState(null, '', url); // Update URL in the browser
        }
    });
}
