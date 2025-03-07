function loadData(url) {
    $('#layoutSidenav_content main').load(url, function (response, status, xhr) {
        if (status === "error") {
            $('#layoutSidenav_content main').html(`<p>Error loading content: ${xhr.statusText}</p>`);
        } else {
            history.pushState(null, '', url); // Update URL in the browser
        }
    });
}

$(document).on('click', '.sb-sidenav-menu .nav-partial', function(e) {
    e.preventDefault();
    const url = $(this).attr('href'); // Construct URL dynamically
    if (url === '#') return;
    loadData(url);
});

