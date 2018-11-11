function MyBlog() {
	var _this = this;

	this.init = function () {
		$("#BlogPostWrapper").jscroll({
			loadingHtml: '<em>Загрузка...</em>',
			nextSelector: "a.next:last",
			getParameters: function () {
				return "skip=" + $("#BlogPostWrapper").data("skip");
			},
			callback: function () {
				$("#BlogPostWrapper").data("skip", $("#LastSkip").val());
				$("#LastSkip").remove();
			}
		});
	}
}

myBlog = null;
$().ready(function ()
{
	myBlog = new MyBlog();
	myBlog.init();
});