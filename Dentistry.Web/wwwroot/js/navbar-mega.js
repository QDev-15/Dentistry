$(document).ready(function () {
    // executes when HTML-Document is loaded and DOM is ready
    var queueAction = [];
    var hoverAny = {
        lv2: null,
        lv3: null
    };
    var intevalResetPrevious = null;
    $('[class*="li-lv2-hover-"]').hover(
        function () {
            // mouse hover
            // resset interval neu co
            if (intevalResetPrevious) {
                clearInterval(intevalResetPrevious);
                intevalResetPrevious = null;
            }
            
            // hiển thị UL
            let matchedClass = $(this).attr("class").match(/li-lv2-hover-(\d+)/);
            if (matchedClass) {
                var index = matchedClass[1]; // Lấy số thứ tự
                if (index != hoverAny.lv2) {
                    if (hoverAny.lv2) {
                        // reset lại previous
                        resetHover(hoverAny);
                    }
                    $(this).addClass("lv2-active");
                    hoverAny.lv2 = index;
                    hoverAny.lv3 = null;
                    // Ẩn tất cả ul-lv3 ngay lập tức
                    $('[class*="ul-lv3-"]').stop(true, true).fadeOut(200);

                    // Hiển thị ul-lv3 tương ứng sau 0.2s
                    setTimeout(() => {
                        if (hoverAny.lv2) {
                            $(".ul-lv3-" + hoverAny.lv2).stop(true, true).fadeIn(200);
                            $(".i-caret-l-" + hoverAny.lv2).stop(true, true).fadeIn(200);
                            $(".i-caret-r-" + hoverAny.lv2).stop(true, true).fadeOut(100);
                            // hiển thị Menu lv3
                            $(".menu-lv3").stop(true, true).fadeIn(200);
                        }
                    }, 200);
                }
            }
        },
        function () {
            // đợi 0.2s nếu không có action thì reset
            fadeOutAny();
        }
    );

    // level 3 hover
    $('[class*="li-lv3"]').hover(
        function () {
            // hover
            hoverAny.lv3 = 'active';
        },
        function () {
            // un-hover
            hoverAny.lv3 = 'non-active';
            fadeOutAny();
        }
    );
    function fadeOutAny() {
        if (intevalResetPrevious) {
            clearInterval(intevalResetPrevious);
            intevalResetPrevious = null;
        }
        intevalResetPrevious = setInterval(() => {
            console.log("lv3 = ", hoverAny.lv3);
            if (hoverAny.lv3 != 'active') {
                resetHover(hoverAny);
            }
            clearInterval(intevalResetPrevious);
            intevalResetPrevious = null;
        }, 200);
    }
    function resetHover(hover) {
        $('[class*="menu-lv3"]').stop(true, true).fadeOut(200);
        if (hover.lv2) {
            $('.li-lv2').removeClass('lv2-active');
            $(".i-caret-l-" + hover.lv2).stop(true, true).fadeOut(100);
            $(".i-caret-r-" + hover.lv2).stop(true, true).fadeIn(100);
        }
    }
    // breakpoint and up  
    $(window).resize(function () {
        initNav();
        
        // when you hover a toggle show its dropdown menu
        $(".navbar .dropdown-toggle").hover(function () {
            var width = $(window).width();
            if (width < 980) {
                return;
            }
            var eWidth = $(window).width()
            $(this).parent().toggleClass("show");
            $(this).parent().find(".dropdown-menu").toggleClass("show");
        });

        // hide the menu when the mouse leaves the dropdown
        $(".navbar .dropdown-menu").mouseleave(function () {
            var width = $(window).width();
            if (width < 980) {
                return;
            }
            $(this).removeClass("show");
        });
    });
    let lastScrollTop = 0;
    const navbar = document.querySelector('.navbar-mega');

    window.addEventListener('scroll', () => {
        const currentScroll = window.pageYOffset;
        //console.log(currentScroll);
        if (currentScroll > 390) {
            // Scrolling down - hide the navbar
            navbar.classList.add('sticky-top');
            setTimeout(() => {
                navbar.classList.add('navbar-mega-visible');
            }, 100);
        }
        if (currentScroll < 100) {
            navbar.classList.remove('sticky-top');
            navbar.classList.remove('navbar-mega-visible');
        }
    });


    initNav();
    function initNav() {
        $(".nav-li-0").removeClass('show');
        $(".nav-li-0 .dropdown-menu").removeClass('show');
        $(".nav-li-0").removeClass('dropdown');
        if ($(window).width() >= 980) {
            $(".nav-li-0").toggleClass('dropdown');
        }
        //$(".navbar .dropdown-toggle").click(function () {
        //    return;
        //});
        
        $(".navbar .nav-li-0 > a").click(function () {
            if ($(window).width() >= 980) { return; }
            queueAction.push(this);
            setTimeout(() => {
                if (queueAction && queueAction.length > 0) {
                    var item = queueAction[queueAction.length - 1];
                    queueAction = [];
                    updateToggle(item);
                }
            }, 100);
        });
    }
    function updateToggle(event) {
        var hasShow = $(event).parent().hasClass('show');
        if (hasShow) {
            $(event).parent().removeClass("show");
            $(event).parent().find(".dropdown-menu").removeClass("show");
            return;
        }
        $(".nav-li-0").removeClass('show');
        $(".nav-li-0 .dropdown-menu").removeClass('show');
        $(event).parent().toggleClass("show");
        $(event).parent().find(".dropdown-menu").toggleClass("show");
        if (queueAction && queueAction.length > 0) {
            var item = queueAction[queueAction.length - 1];
            queueAction = [];
            updateToggle(item);
        }
    }
});

// Toggle the icon direction when the collapsible content is shown/hidden
document.querySelectorAll('.expand-btn').forEach(button => {
    button.addEventListener('click', function () {
        const icon = this.querySelector('i');
        const isChevronDown = icon.classList.contains('fa-chevron-down');

        if (isChevronDown) {
            icon.classList.remove('fa-chevron-down');
            icon.classList.add('fa-chevron-up');
        } else {
            icon.classList.remove('fa-chevron-up');
            icon.classList.add('fa-chevron-down');
        }
    });
});