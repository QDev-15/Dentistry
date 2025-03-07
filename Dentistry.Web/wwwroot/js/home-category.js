$(document).ready(function () {
    function categorySlider() {
        //var slidesToShow = 4;
        //let savedScreenSize = getCookie("screenCategorySize");
        //var screen = getBootstrapBreakpoint();
        //if (screen != savedScreenSize) {
        //    if (screen == 'sm') {
        //        slidesToShow = 3;
        //        // Lưu vào cookie (giữ 7 ngày)
        //        setCookie("screenCategorySize", screen, 7);
        //    } else if (screen == 'xs') {
        //        slidesToShow = 2;
        //        // Lưu vào cookie (giữ 7 ngày)
        //        setCookie("screenCategorySize", screen, 7);
        //    } else {
        //        slidesToShow = 4;
        //        // Lưu vào cookie (giữ 7 ngày)
        //        setCookie("screenCategorySize", screen, 7);
        //    }
            
        //}
        $('.actor-list').slick({
            slidesToShow: 4, // Number of items visible at once
            slidesToScroll: 1,
            autoplay: false,
            autoplaySpeed: 2000,
            dots: false,
            arrows: false, // Hide navigation arrows
            responsive: [
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 2
                    }
                }
            ]
        });
    }
    categorySlider();
});




