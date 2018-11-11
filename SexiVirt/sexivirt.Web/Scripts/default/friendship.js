function Friendship()
{
    var _this = this;

    this.init = function () {
        $(".addToFriendship").click(function ()
        {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Friend/InviteToFriend",
                data: { id: id },
                success: function (data) {
                    if (data.result = "ok") {
                        window.location.reload();
                    };
                }
            });
        });

        $(".cancelFriendship").click(function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Friend/RemoveFriend",
                data: { id: id },
                success: function (data) {
                    if (data.result = "ok") {
                        window.location.reload();
                    };
                }
            });
        });

        $(".confirmFriendship").click(function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Friend/ConfirmFriendship",
                data: { id: id },
                success: function (data) {
                    if (data.result = "ok") {
                        window.location.reload();
                    };
                }
            });
        });

        $(".declineFriendship").click(function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Friend/DeclineFriendship",
                data: { id: id },
                success: function (data) {
                    if (data.result = "ok") {
                        window.location.reload();
                    };
                }
            });
        });


        $(".blockUser").click(function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Friend/BlockUser",
                data: { id: id },
                success: function (data) {
                    if (data.result = "ok") {
                        window.location.reload();
                    };
                }
            });
        });

        $(".unblockUser").click(function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Friend/UnblockUser",
                data: { id: id },
                success: function (data) {
                    if (data.result = "ok") {
                        window.location.reload();
                    };
                }
            });
        });
    }
}


var friendship = null;
$().ready(function () {
    friendship = new Friendship();
    friendship.init();
})