function Conversation() {
    var _this = this;

    this.stop = false;
    this.stopSend = false;
    this.init = function () {
        //var height = $("#MessagesWrapper")[0].scrollHeight - $("#MessagesWrapper").outerHeight(false);

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


/*
        $("#MessagesWrapper").scroll(function () {
            var top = $("#MessagesWrapper").scrollTop();
            console.log("Top: " + top);
            var marker = $("#LoadMarker");
            var skip = marker.data("skip");
            if (top < 200 && !_this.stop && marker.length > 0) {
                var bottom = $("#MessagesWrapper")[0].scrollHeight - top;
                console.log("Bottom: " + bottom);
                $.ajax({
                    type: "GET",
                    url: "/Message/Load",
                    data: {
                        id: $("#ID").val(),
                        skip: skip
                    },
                    beforeSend: function () {
                        _this.stop = true;
                        $("#LoadMarker").remove();
                    },

                    success: function (data) {
                        if (data) {
                            $("#MessagesWrapper").scrollTop(height);
                            $("#MessagesWrapper").prepend(data);
                            var height = $("#MessagesWrapper")[0].scrollHeight - bottom;
                            console.log("Height: " + height);
                            $("#MessagesWrapper").scrollTop(height);
                            _this.stop = false;
                        }
                    },
                    error: function () {
                        _this.stop = false;
                    }
                });
            }
        });
*/

        $("#WriteMessageBtn").click(function ()
        {
            _this.sendMessage();
        });

        $("#Message").keypress(function (e) {
            if (e.which == 13) {
                _this.sendMessage();
            }
        });

        $(document).on("click", ".dialog-item .delete-icon", function () {
            var id = $(this).closest(".dialog-item").data("id");
            $.ajax({
                type: "GET",
                url: "/Message/RemoveMessage",
                data: { id: id },
                success: function (data) {
                    if (data.result == "ok") {
                        $(".dialog-item[data-id='" + id + "']").remove();
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
                url: "/Message/GetLast",
                beforeSend: function() {
                    _this.stopSend = true;
                },
                data: {
                    id: $("#ID").val(),
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
            url: "/Message/ReadAll",
            data: {
                id: $("#ID").val()
            },
            success: function (data)
            { }
        });
    }

    this.sendMessage = function () {
        if (!_this.stopSend && $("#Message").val() != "") {
            $.ajax({
                type: "POST",
                url: "/Message/WriteMessage",
                beforeSend: function () {
                    _this.stopSend = true;
                },
                data: {
                    id: $("#ID").val(),
                    text: $("#Message").val(),
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
        setTimeout(UpdateConversation, 5000);
    }
}

var conversation = null;
$().ready(function () {
    conversation = new Conversation();
    conversation.init();

    SendUpdateConversation();
});