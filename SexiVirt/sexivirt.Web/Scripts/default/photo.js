function Photo() {
    var _this = this;

    this.init = function () {
        $(document).on("click", ".albumPhotoItem", function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Album/Photo",
                data: { id: id },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
        });

        $(document).on("click", ".userPhotoItem", function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Album/AllPhoto",
                data: { id: id },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
        });

        $(document).on("click", ".changePict", function () {
            var id = $(this).data("id");
            var next = $(this).data("next");
            $.ajax({
                type: "GET",
                url: "/Album/ChangePhoto",
                data: { id: id, next : next },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
        });
        
        $(document).on("click", ".changePictUser", function () {
            var id = $(this).data("id");
            var next = $(this).data("next");
            $.ajax({
                type: "GET",
                url: "/Album/ChangeAllPhoto",
                data: { id: id, next: next },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
        });
    }
}

var photo = null;
$().ready(function () {
    photo = new Photo();
    photo.init();
})