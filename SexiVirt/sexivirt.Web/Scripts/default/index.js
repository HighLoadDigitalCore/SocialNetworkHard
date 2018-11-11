function Index() {
    var _this = this;

    this.init = function () {
        initLoginBtns();

    }
}

function initLoginBtns() {
    $("a[rel^='prettyPhoto']").prettyPhoto({ theme: 'facebook' });


    $("#ShowRegisterButton").unbind('click');
    $("#ShowSocialButton").unbind('click');
    $("#pass").unbind('click');
    $("#ShowRegisterButton").click(function () {
        $("#RegisterSocialWrapper").hide();
        $('#PasswordWrapper').hide();
        $("#RegisterWrapper").toggle();
    });

    $("#ShowSocialButton").click(function () {
        $("#RegisterWrapper").hide();
        $('#PasswordWrapper').hide();
        $("#RegisterSocialWrapper").toggle();
    });

    $("#pass").click(function () {
        $("#RegisterWrapper").hide();
        $('#PasswordWrapper').toggle();
        $("#RegisterSocialWrapper").hide();
        return false;
    });
}

var index = null;
$().ready(function () {
    index = new Index();
    index.init();
    var totalWidth = $('.middle').innerWidth();
    try {
        var item0Width = $('.carousel0 .carouselWrap li').width();
        var item0Count = parseInt(totalWidth / item0Width);
        var items0Calc = item0Count * item0Width;
        $('.carousel0 .carouselWrap').css('width', items0Calc + "px");
        $('.carousel0 .carouselWrap').jcarousel({ scroll: 1, visible: item0Count, wrap: 'circular' });
        $('.carousel0 .carouselPrev').click(function () {
            $('.carousel0 .carouselWrap').jcarousel('scroll', '-=1');
        });

        $('.carousel0 .carouselNext').click(function () {
            $('.carousel0 .carouselWrap').jcarousel('scroll', '+=1');
        });

    }
    catch (e) { }
    try {
        var item1Width = $('.carousel1 .carouselWrap .event-item').width() + 42;
        var item1Count = parseInt(totalWidth / item1Width);
        var items1Calc = item1Count * item1Width;
        $('.carousel1 .carouselWrap').css('width', items1Calc + "px");

        $('.carousel1 .carouselWrap').jcarousel({ scroll: 1, visible: item1Count, wrap: 'circular' });
        $('.carousel1 .carouselPrev1').click(function () {
            $('.carousel1 .carouselWrap').jcarousel('scroll', '-=1');
        });

        $('.carousel1 .carouselNext1').click(function () {
            $('.carousel1 .carouselWrap').jcarousel('scroll', '+=1');
        });
    }
    catch (e) { }


})