

    $(document).on('click', '.sb-sidenav-menu .nav-partial', function(e) {
        e.preventDefault();
        const url = $(this).attr('href'); // Construct URL dynamically
        if (url === '#') return;
        loadData(url);
    });

