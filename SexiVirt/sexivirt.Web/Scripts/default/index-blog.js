function IndexBlog() {
    var _this = this;

    this.init = function () {

        $('.diaries .cd_filters span').click(function() {
            $('.diaries .cd_filters span').addClass('active');
            $(this).removeClass('active');
            $("#BlogUserWrapper").data("skip", '18');
            $('#BlogUserWrapper').jscroll.destroy();
            fillList();
            
            return false;
        });


        function fillList() {
            $.ajax({
                type: "POST",
                url: "/Blog/IndexLoad",
                data: { type: $('.diaries .cd_filters span').not('.active').data('type'), skip:0 },
                success: function (data) {
                    $("#BlogUserWrapper").html(data);
                    loadList();
                }
            });
        }

        loadList();

        function loadList() {
            $("#BlogUserWrapper").jscroll({
                loadingHtml: '<em>Загрузка...</em>',
                nextSelector: "a.next:last",
                getParameters: function () {
                    return "skip=" + $("#BlogUserWrapper").data("skip") + "&type=" + $('.diaries .cd_filters span').not('.active').data('type');
                },
                callback: function () {
                    
                    $("#BlogUserWrapper").data("skip", $("#LastSkip").val());
                    $("#LastSkip").remove();
                },
                autoTrigger: true,
                refresh: true
            });
        }
    }
}

indexBlog = null;
$().ready(function () {

    indexBlog = new IndexBlog();
    indexBlog.init();
});