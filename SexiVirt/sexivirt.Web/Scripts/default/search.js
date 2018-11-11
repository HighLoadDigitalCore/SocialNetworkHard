function Search() {
    var _this = this;

    this.init = function () {
        var word = $("#searchString").val();
        $(".search-results").highlight(word);


        $(document).on("click", ".dots", function () {
            var parent = $(this).closest(".cam_services");
            var id = $(this).data("id");
            var opened = $(this).data("opened");

            $.ajax({
                type: "GET",
                url: "/People/UserPreference",
                data: { id: id, opened:  opened},
                success: function (data) {
                    parent.html(data);
                }
            });
        });

    }
}


var search = null;
$().ready(function () {
    search = new Search();
    search.init();
})