function BlogBlog()
{
    var _this = this;

    this.init = function ()
    {
        $("#BlogPostWrapper").jscroll({
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            getParameters: function () {
                return "skip=" + $("#BlogPostWrapper").data("skip") + "&id=" + $("#BlogPostWrapper").data("id");
            },
            callback: function () {
                $("#BlogPostWrapper").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
            }
        });
    }
}

var blogBlog = null;
$().ready(function () {
    blogBlog = new BlogBlog();
    blogBlog.init();
});