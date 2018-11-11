function FeedIndex() {
    var _this = this;

    this.init = function () {
        $("#FeedWrapper").jscroll({
            debug: true,
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            callback: function () {
                $("#EventResult").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
                _this.updateCount();
            }
        });

        $(".news-content .comment").click(function () {
            var url = $(this).data("url");
            var commentId = $(this).data("commentId");

            window.location = url + "?commentId=" + commentId;
        });
    }

    this.updateCount = function ()
    {
        $.ajax({
            type: "GET",
            url: "/Feed/Count",
            success: function (data) {
                $("#feedCountWrapper").replaceWith(data);
            }
        });
    }
}

var feedIndex = null;
$().ready(function () {
    feedIndex = new FeedIndex();
    feedIndex.init();
});