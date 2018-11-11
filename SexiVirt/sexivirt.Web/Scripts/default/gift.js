function Gift() {
    var _this = this;

    this.init = function () {
        $("#MakeGiftBtn").click(function () {
            var id = $(this).data("id");
            _this.showPopup(id);
        });

        $("#MakeGiftAnchor").click(function () {
            var id = $(this).data("id");
            _this.showPopup(id);
            return false;
        });

        $(document).on("click", ".switchGiftList", function () {
            var type = $(this).data("type");
            $(".switchGiftList").removeClass("active");
            $(this).addClass("active");
            $("#GiftType").val(type);
            $.ajax({
                type: "GET",
                url: "/Gift/List",
                data: { type: type },
                success: function (data) {
                    $("#GiftListWrapper").html(data);
                    if ($(window).height() < 800) {
                        $('#GiftListWrapper .jScrollPaneContainer').css('height', '120px');
                        $('#GiftListWrapper').css('height', '160px');
                    }
                    //$('#pane').jScrollPane({ scrollbarWidth: 8, showArrows: true });
                    $('#GiftListWrapper .jScrollPaneContainer').mCustomScrollbar();
                }
            });
        });

        $(document).on("click", ".giftItem", function () {
            var id = $(this).data("id");
            $(".giftItem").removeClass("active");
            $(this).addClass("active");
            $("#GiftID").val(id);
        });

        $(document).on("click", ".giftUserItem", function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Gift/Item",
                data: { id: id },
                success: function (data)
                {
                    $("#PopupWrapper").html(data);
                    $('#PopupWrapper').find('.popup.gifts .dialog-item.user-1 .body .text.delete .text-wrapper').mCustomScrollbar();
                }
            });
        });

        $(document).on("click", ".deleteUserGift", function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Gift/DeleteUserGift",
                data: { id: id },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
        });
    }

    this.showPopup = function (id) {
        $.ajax({
            type: "GET",
            url: "/Gift/Popup",
            data: { id: id },
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
                
            }
        })
    }

    this.initPopup = function () {
        //$('#pane').jScrollPane({ scrollbarWidth: 8, showArrows: true });
        if ($(window).height() < 800) {
            $('#GiftListWrapper .jScrollPaneContainer').css('height', '120px');
            $('#GiftListWrapper').css('height', '160px');
        }

        $('#GiftListWrapper .jScrollPaneContainer').mCustomScrollbar();
        $("#Visible").styler();

        $("#MakeGiftSubmit").click(function () {
            $.ajax({
                type: "POST",
                url: $("#MakeGiftForm").attr("action"),
                data: $("#MakeGiftForm").serialize(),
                success: function (data) {
                    $("#PopupWrapper").html(data);
                    if ($("#pane").length > 0) {
                        _this.initPopup();
                    }
                }
            });
            return null;
        });
    }
}

var gift = null;
$().ready(function () {
    gift = new Gift();
    gift.init();
});