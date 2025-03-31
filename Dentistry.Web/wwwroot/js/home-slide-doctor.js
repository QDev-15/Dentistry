$(document).ready(function () {
    var totalSlides = $('.slide-item').length;
    $('.doctor-list-item').slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: false,
        autoplaySpeed: 2000,
        dots: false,
        arrows: true,
        infinite: totalSlides > 4,
        centerMode: true,
        centerPadding: '0px',
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 3,
                    infinite: totalSlides > 3,
                    centerMode: true,
                    centerPadding: '40px' // Căn giữa khi chỉ có một item
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 2,
                    infinite: totalSlides > 2,
                    centerMode: true,
                    centerPadding: '40px' // Căn giữa khi chỉ có một item
                }
            },
            {
                breakpoint: 1,
                settings: {
                    slidesToShow: 1,
                    infinite: totalSlides > 1,
                    centerMode: true,
                    centerPadding: '40px' // Căn giữa khi chỉ có một item
                }
            }
        ]
    });
    let myCarousel = new bootstrap.Carousel($('#carouselExampleAutoplaying')[0]);

    // Khi click vào item, set nó làm current slide
    $('.doctor-list-item').on('click', '.slide-item', function () {
        let index = $(this).closest('.slick-slide').data('slide-item');
        // chuyển đến slick item selected
        $('.doctor-list-item').slick('slickGoTo', index);
        // chuyển đến slide tương ứng với slide
        myCarousel.to(index);
    });
    // click dot slide trên tự động chọn item dưới
    $('#carouselExampleAutoplaying').on('click', '.carousel-li-item', function () {
        let index = $(this).data('bs-slide-to');
        // chuyển đến slick item selected
        $('.doctor-list-item').slick('slickGoTo', index);
    });

    // Khi slide thay đổi, lấy index của slide hiện tại
    $('#carouselExampleAutoplaying').on('slid.bs.carousel', function (event) {
        let currentIndex = event.to;
        // chuyển đến slick item selected
        $('.doctor-list-item').slick('slickGoTo', currentIndex);
    });
});


//const carouselElement = document.getElementById('carouselExampleAutoplaying');
//const slideItems = document.querySelectorAll('.slide-item');
//let touchStartX = 0;
//let touchEndX = 0;
//let mouseStartX = 0;
//let mouseEndX = 0;

//// Cập nhật trạng thái active của danh sách bài viết
//function updateSlideList(activeIndex) {
//    const items = document.querySelectorAll('.slide-item');
//    var itemActive = null;
//    items.forEach((item, index) => {
//        item.classList.remove('active'); // Loại bỏ class 'active' khỏi tất cả items
//        if (item.classList.contains('slide-item-' + activeIndex)) { // Kiểm tra class
//            item.classList.add('active'); // Thêm class 'active' vào item phù hợp
//            itemActive = item;
//            $('.slider').slick('slickGoTo', activeIndex);
//        }
//    });
    
    
    
//}

//// Lắng nghe sự kiện chuyển slide
//carouselElement.addEventListener('slid.bs.carousel', (event) => {
//    const activeIndex = event.to; // Lấy index của slide active
//    updateSlideList(activeIndex);
//});
//// Bắt đầu sự kiện touch
//carouselElement.addEventListener('touchstart', (event) => {
//    touchStartX = event.changedTouches[0].screenX;
//});

//// Kết thúc sự kiện touch
//carouselElement.addEventListener('touchend', (event) => {
//    touchEndX = event.changedTouches[0].screenX;
//    handleSwipeGesture();
//});

//// Bắt đầu sự kiện mouse (mousedown)
//carouselElement.addEventListener('mousedown', (event) => {
//    mouseStartX = event.clientX;
//});

//// Kết thúc sự kiện mouse (mouseup)
//carouselElement.addEventListener('mouseup', (event) => {
//    mouseEndX = event.clientX;
//    handleMouseSwipeGesture();
//});

//// Xử lý vuốt cảm ứng
//function handleSwipeGesture() {
//    if (touchEndX < touchStartX) {
//        // Vuốt sang trái
//        const carouselInstance = bootstrap.Carousel.getInstance(carouselElement);
//        carouselInstance.next();
//    }
//    if (touchEndX > touchStartX) {
//        // Vuốt sang phải
//        const carouselInstance = bootstrap.Carousel.getInstance(carouselElement);
//        carouselInstance.prev();
//    }
//}

//// Xử lý vuốt chuột
//function handleMouseSwipeGesture() {
//    if (mouseEndX < mouseStartX) {
//        // Kéo chuột sang trái
//        const carouselInstance = bootstrap.Carousel.getInstance(carouselElement);
//        carouselInstance.next();
//    }
//    if (mouseEndX > mouseStartX) {
//        // Kéo chuột sang phải
//        const carouselInstance = bootstrap.Carousel.getInstance(carouselElement);
//        carouselInstance.prev();
//    }
//}

/////////////////// ============== SUBSLIDE ================== //////////////////


//let slideItemMouseStart = 0;
//let slideItemMouseEnd = 0;
//// Kết nối danh sách bài viết với sự kiện click
//slideItems.forEach((item, index) => {
//    // Bắt đầu sự kiện mouse (mousedown)
//    item.addEventListener('mousedown', (event) => {
//        slideItemMouseStart = event.clientX;
//    });

//    // Kết thúc sự kiện mouse (mouseup)
//    item.addEventListener('mouseup', (event) => {
//        slideItemMouseEnd = event.clientX;
//        if (slideItemMouseStart == slideItemMouseEnd) {
//            updateCarouselElement(index);
//        }
//    });
//    // Bắt đầu sự kiện touch
//    carouselElement.addEventListener('touchstart', (event) => {
//        slideItemMouseStart = event.changedTouches[0].screenX;
//    });

//    // Kết thúc sự kiện touch
//    carouselElement.addEventListener('touchend', (event) => {
//        slideItemMouseEnd = event.changedTouches[0].screenX;
//        if (slideItemMouseStart == slideItemMouseEnd) {
//            updateCarouselElement(index);
//        }
//    });
//});

//function updateCarouselElement(index) {
//    const carouselInstance = bootstrap.Carousel.getOrCreateInstance(carouselElement);
//    carouselInstance.to(index); // Chuyển đến slide tương ứng
//    updateSlideList(index); // Cập nhật trạng thái danh sách bài viết
//}




