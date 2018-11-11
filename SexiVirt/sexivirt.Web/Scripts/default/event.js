function EventIndex() {
    var _this = this;

    this.init = function ()
    {
        $("#SearchBtn").click(function () {
            $("#SearchEventForm").submit();
        })

        $(".selectEmul").styler({
            selectSmartPositioning: false,
            selectSearch: false,
            selectVisibleOptions: 6,
            singleSelectzIndex: 2
        });


        $("#EventResult").jscroll({
            debug : true,
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            getParameters: function () {
                return $("#SearchEventForm").serialize() + "&skip=" + $("#EventResult").data("skip");
            },
            callback: function () {
                $("#EventResult").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
            }
        });
    }
}

var eventIndex = null;
$().ready(function ()
{
    eventIndex = new EventIndex();
    eventIndex.init();
});