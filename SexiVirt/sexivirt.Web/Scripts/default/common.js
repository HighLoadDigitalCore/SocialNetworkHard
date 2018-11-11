function loadPayItems() {
    $('.popup.balance .item a').click(function () {
        $('.popup.balance .item a').removeClass('active');
        $(this).addClass('active');
        $('#PayType').val($(this).data('arg'));
        return false;
    });

    $('#RemoveCell button').click(function () {
        $('#WF').submit();
    });
}

function showMoneyRefill() {
    $.ajax({
        type: "GET",
        url: "/Money/Popup",
        success: function (data) {
            $("#PopupWrapper").html(data);
            loadSwitchers();

        }
    });
}

$().ready(function () {
    $(document).on("click", "#DiaryFullView", function () {
        $('#PostFullText').html($('#PostFull').val());
        $(this).hide();
        return false;
    });
    $(document).on("click", ".carousel0 li", function () {
        var id = $(this).data("id");

        $.ajax({
            type: "GET",
            url: "/Meeting/Popup",
            data: { id: id },
            success: function (data) {
                $("#PopupWrapper").html(data);
            }
        });
        return false;
    })

    $('.social iframe').css('width', '125px').css('height', '36px');

    $('#MessagePay').click(function () {
        $.ajax({
            type: "GET",
            url: "/Money/PayMessage",
            success: function (data) {
                $("#PopupWrapper").html(data);


            }
        });
        return false;
    });
})

function loadSwitchers() {
    $('.popup.balance .popup-tabs span').click(function () {
        var $t = $(this);
        $.ajax({
            type: "GET",
            url: "/Money/" + $t.attr('id'),
            success: function (data) {
                $("#PopupContent").html(data);
                $('.popup.balance .popup-tabs span').removeClass('active');
                $t.addClass('active');

                loadPayItems();

                var h = $('.money-history');
                if (h.length) {
                    h.mCustomScrollbar({ advanced: { updateOnContentResize: true, updateOnBrowserResize: false } });
                }
            }
        });


    });
}

function Common() {
    var _this = this;

    this.init = function () {

        $(document).on("click", "#MoneyBtn, #RefillBtn", function () {
            $.ajax({
                type: "GET",
                url: "/Money/Popup",
                success: function (data) {
                    $("#PopupWrapper").html(data);
                    loadSwitchers();

                }
            });
        });


        $(document).on("click", ".delete-action", function () {
            return confirm("Вы действительно хотите удалить?");
        });

        $(document).ready(function () {
            $(".selectEmul").styler({
                selectSearch: false,
                onSelectOpened: function () {

                    $(this).find('.jq-selectbox__dropdown').mCustomScrollbar("update");
                }
            });

            setTimeout(function () {

                $('.selectEmul .jq-selectbox__dropdown').mCustomScrollbar({
                    advanced: {
                        updateOnContentResize: true,
                        updateOnBrowserResize: false
                    },


                });
            }, 500);

        });
        /*
                setTimeout(function() {
                    $('.max-height-select .jq-selectbox__dropdown ul').jScrollPane();
                }, 1000);
        */


        /*
                $(function () {
                    $('.paralax').mousemove(function (e) {
                        if (Math.random() > 0.5) {
                            var x = e.clientX - $(window).width() / 2;
                            var y = e.clientY - $(window).height() / 2;
                            $('.paralax > div > div:eq(0)').css({ 'left': x / 50 - 1024, 'top': y / 50 - 76 });
                            $('.paralax > div > div:eq(1)').css({ 'left': x / 25 - 930, 'top': y / 25 - 270 });
                            $('.paralax > div > div:eq(3)').css({ 'left': x / 10 - 930, 'top': y / 10 - 93 });
                        }
                    })
                })
        */

        $(function () {
            $('.paralax').mousemove(function (e) {
                if (Math.random() > 0.5) {
                    var x = e.clientX - $(window).width() / 2;
                    var y = e.clientY - $(window).height() / 2;
                    $('.paralax > div > div:eq(0)').css({ 'left': x / 50 - 1050, 'top': y / 15 - 76 });
                    $('.paralax > div > div:eq(1)').css({ 'left': x / 25 - 1100, 'top': y / 30 - 270 });
                    $('.paralax > div > div:eq(3)').css({ 'left': x / 10 - 1250, 'top': y / 45 - 93 });
                }
            })
        })

        $(document).on("click", ".pp_close, .close-popup", function () {
            $(".popupBg").remove();
            $(".popup").remove();
            return false;
        });

        $("#searchBtnSearch").click(function () {
            $("#searchForm").submit();
        });
    }
}


function getURLParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null
}

function UpdateOnline() {
    SendUpdateOnline();
}

function SendUpdateOnline() {
    $.getJSON("/Home/Online/", function (data) {
        if (data.result == 'lock') {
            document.location.href = '/';
        }
    });
    setTimeout(UpdateOnline, 60000);
}

var common = null;
$(function () {
    common = new Common();
    common.init();
    SendUpdateOnline();
});