function Rating() {
    var _this = this;

    this.init = function () {
        $(document).on("click", ".voteForUser", function () {
            var id = $(this).data("id");
            var mark = $(this).data("mark");

            $.ajax({
                type: "GET",
                url: "/User/Rating",
                data: {
                    id: id,
                    mark : mark
                },
                success: function (data) {
                    $("#RatingWrapper").html(data);
                }
            })
        })
    }
}

var rating = null;
$().ready(function () {
    rating = new Rating();
    rating.init()
});