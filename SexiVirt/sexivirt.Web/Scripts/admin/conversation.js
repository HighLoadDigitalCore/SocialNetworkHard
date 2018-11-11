function Conversation() {
    var _this = this;
    this.timer = null;
    this.stop = false;
    this.stopSend = false;
    this.init = function () {
        //var height = $("#MessagesWrapper")[0].scrollHeight - $("#MessagesWrapper").outerHeight(false);
        $(window).unbind('resize');
        $(window).resize(function() {
            $('#MessagesWrapper').css('max-height', $(window).height() - 390);
            $('#MessagesWrapper').mCustomScrollbar('update');
            $('#MessagesWrapper').mCustomScrollbar('scrollTo', '#LastLine');
        });

        $('#MessagesWrapper').css('max-height', $(window).height() - 390);

        $('#MessagesWrapper').mCustomScrollbar({
            advanced: {
                updateOnBrowserResize: false,
                updateOnContentResize: false
            }
        });

        
        $('#MessagesWrapper').mCustomScrollbar('scrollTo', '#LastLine', { scrollInertia: 0 });

        $("#WriteMessageBtn").unbind('click');
        $("#WriteMessageBtn").click(function ()
        {
            _this.sendMessage();
        });
        $("#Message").unbind('keypress');
        $("#Message").keypress(function (e) {
            if (e.which == 13) {
                _this.sendMessage();
            }
        });
        $(".dialog-item .delete-icon").unbind('click');
        $(document).on("click", ".dialog-item .delete-icon", function () {
            var id = $(this).closest(".dialog-item").data("id");
            $.ajax({
                type: "GET",
                url: "/admin/Chat/RemoveMessage",
                data: { id: id, ChatUserID: $('#ChatUserID').val() },
                success: function (data) {
                    if (data.result == "ok") {
                        $(".dialog-item[data-id='" + id + "']").remove();
                        $('#MessagesWrapper').mCustomScrollbar('update');
                        $('#MessagesWrapper').mCustomScrollbar('scrollTo', '#LastLine');

                    }
                }
            });
        });
    }

    this.getLast = function (override) {
        if (!_this.stopSend || override) {
            var id = $(".dialog-item:last").data("id");
            $.ajax({
                type: "GET",
                url: "/admin/Chat/GetLast",
                beforeSend: function() {
                    _this.stopSend = true;
                },
                data: {
                    id: $("#ID").val(),
                    ChatUserID: $('#ChatUserID').val(),
                    idLastMessage: id,
                },
                success: function(data) {
                    if (data.length) {
                        $("#MessagesList").append(data);

/*
                var height = $("#MessagesWrapper")[0].scrollHeight - $("#MessagesWrapper").outerHeight(false);
                $("#MessagesWrapper").scrollTop(height);
*/
                        $('#MessagesWrapper').mCustomScrollbar('update');
                        $('#MessagesWrapper').mCustomScrollbar('scrollTo', '#LastLine');
                    }
                    _this.readConversation();

                },
                complete: function () {
                    _this.stopSend = false;
                }
            });
        }
    }

    this.readConversation = function () {
        $.ajax({
            type: "GET",
            url: "/admin/Chat/ReadAll",
            data: {
                id: $("#ID").val(),
                ChatUserID: $('#ChatUserID').val()
            },
            success: function (data)
            { }
        });
    }

    this.sendMessage = function () {
        if (!_this.stopSend && $("#Message").val() != "") {
            $.ajax({
                type: "POST",
                url: "/admin/Chat/WriteMessage",
                beforeSend: function () {
                    _this.stopSend = true;
                },
                data: {
                    id: $("#ID").val(),
                    text: $("#Message").val(),
                    ChatUserID: $('#ChatUserID').val()
                },
                success: function (data) {
                    if (data.result == "ok") {
                        _this.getLast(true);
                    }

                    $("#Message").val("");
                },
                complete: function () {
                    _this.stopSend = false;
                }
            });
        }
    }
}

function UpdateConversation() {
    SendUpdateConversation();
}

function SendUpdateConversation() {
    if (conversation != null) {
        conversation.getLast();
        if (conversation.timer) {
            clearTimeout(conversation.timer);
        }
        conversation.timer = setTimeout(UpdateConversation, 5000);
    }
}

var conversation = null;
$().ready(function () {
    conversation = new Conversation();
    conversation.init();

    SendUpdateConversation();
});