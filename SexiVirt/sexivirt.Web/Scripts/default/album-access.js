function AlbumAccess()
{
    var _this = this;

    this.init = function () {
        $(".payForLook").click(function () {
            var id = $(this).data("id");
            
            $.ajax({
                type: "GET",
                url: "/Album/BuyAlbum",
                data: { id: id },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
            return false;
        });

        $(document).on("click", "#OpenAlbumBtn", function () {
            var id = $(this).data("id");
            var userid = $(this).data("userid");
            $.ajax({
                type: "POST",
                url: "/Album/BuyAlbum",
                data: {
                    id: id,
                    userId: userid
                },
                success: function (data) {

                    if (data.result == "ok") {
                        window.location = "/Album/Item/" + data.id;
                    }
                    if (data.result == "need-money") {
                        money.showPopup();
                    }
                }
            });
        })
    }
}

var albumAccess = null;
$().ready(function () {
    albumAccess = new AlbumAccess();
    albumAccess.init();
});