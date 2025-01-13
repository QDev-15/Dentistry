$(document).ready(function () {
    // Initialize Slick Slider
    $('.slider-service').slick({
        infinite: true,
        slidesToShow: 4,  // Adjust the number of slides shown based on screen size
        slidesToScroll: 1,
        arrows: true, // Enable next/prev buttons
        dots: false,   // Show navigation dots
        autoplay: false,
        autoplaySpeed: 3000, // Auto-slide every 3 seconds
        prevArrow: '<button type="button" class="slick-prev"><i class="fa-solid fa-angle-left"></i></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa-solid fa-angle-right"></i></i></button>',
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,  // Show 1 slide on mobile screens
                }
            }
        ]
    });
});

