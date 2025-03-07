$(document).ready(function () {
    // executes when HTML-Document is loaded and DOM is ready
    var queueAction = [];
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
        console.log(currentScroll);
        if (currentScroll > 190) {
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
    // document ready  
});