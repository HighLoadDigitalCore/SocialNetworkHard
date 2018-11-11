function Message()
{
    var _this = this;

    this.init = function () {
        $("#ConnectsWrapper").jscroll({
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last"
        });

        $("#MessageSearch").keyup(function () {
            _this.filter();
        })
    }

    this.filter = function()
    {
        var filter = $("#MessageSearch").val();

        $.ajax({
            type: "GET",
            url : "/Message/LoadConversations",
            data: { filter: filter },
            success: function (data)
            {
                $("#ConnectsWrapper").html(data);
            }
        })
    }
}

var message = null;
$().ready(function () {
    message = new Message();
    message.init();
})