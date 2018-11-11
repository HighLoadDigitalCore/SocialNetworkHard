function IndexGroup() {
    var _this = this;

    this.init = function () {
        $("#GroupListWrapper").jscroll({
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            getParameters: function () {
                return $("#GroupSearchForm").serialize() + "&skip=" + $("#GroupListWrapper").data("skip") + "&SearchString=" + $("#SearchString").val();
            },
            callback: function () {
                $("#GroupListWrapper").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
            }
        });


       
        try {
            $("#SearchString").donetyping(function() {
                var data = $("#SearchString").val();
                console.log("Search:" + data);
                _this.searchGroups(data);
            }, 500);
        } catch (e) {
            
        }

    }

    this.searchGroups = function (searchString) {
        $.ajax({
            type: "GET",
            url: "/Group/Load",
            data: { SearchString: searchString, skip:0 },
            success: function (data) {
                $("#GroupListWrapper").html(data);
            }
        })
    }

}

indexGroup = null;
$().ready(function () {

    indexGroup = new IndexGroup();
    indexGroup.init();
});