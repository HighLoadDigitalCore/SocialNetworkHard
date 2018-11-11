function isFunction(functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
}

function Common() {
    var _this = this;

    this.init = function () {
        $(document).on("click", ".delete-action, .btn-danger", function () {
            return confirm("Вы действительно хотите удалить?");
        });

        $(function () { // will trigger when the document is ready
            $('.datepicker').datepicker({ format: "dd.mm.yyyy" }); //Initialise any date pickers
        });


        try {

            $('.chat-add input').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/admin/Chat/SelectUser",
                        data: {
                            term: request.term
                        },
                        success: function (data) {
                            response($.map(data.data, function (item) {
                                return {
                                    label: item.name,
                                    value: item.name,
                                    id: item.id
                                }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    $('.chat-add input').val(ui.item.label);
                    $('.chat-add input').attr('arg', ui.item.id);
                    return false;
                }
            });
        }

        catch (e) {

        }
    }
}

function showChat(uid) {
    
    $.post('/admin/Chat/IndexChat', { ChatUserID: uid }, function (d) {
        $('#ChatWindow').html(d);
        $('.user-chat').removeClass('active-link');
        $('.user-chat[uid="' + uid + '"]').addClass('active-link');
        loadChatScripts();
    });

}

function removeConversation(obj, id) {
    if (conversation != null) {
        if (conversation.timer) {
            clearTimeout(conversation.timer);
        }
    }
    $.get($(obj).attr('href'), { ChatUserID: $('#ChatUserID').val() }, function (d) {
        $('.messages-item[arg="' + id + '"]').remove();
        $('#ChatWrapper').html('');
    });

}

function loadConversation(obj) {

    $.get($(obj).attr('href'), {}, function (d) {
        $('#ConnectsWrapper .title a').removeClass('active-link');
        $(obj).addClass('active-link');
        $('#ChatWrapper').html(d);
        loadChatMsgScripts();
        scrollTo(0, 0);
    });

}

function loadChatMsgScripts() {
    conversation = new Conversation();
    conversation.init();

    SendUpdateConversation();

};

function loadChatScripts() {
    loadMessage();
}

function addUserToChat() {
    $.post('/admin/Chat/AddUser', { userid: $('.chat-add input').attr('arg') }, function (d) {
        $('#chatList ul li').not('.skip').remove();
        $('#chatList ul li.skip:first').after(d);
        $('.chat-add input').val('');
        $('.chat-add input').attr('arg', '');
    });
}

function deleteUser(uid) {
    $.post('/admin/Chat/DeleteUser', { userid: uid }, function (d) {
        $('#chatList ul li').not('.skip').remove();
        $('#chatList ul li.skip:first').after(d);
    });

}

var rt = null;
function refreshChat() {
    if ($('#adminChatAdd').length) {
        $.get('/admin/Chat/StatusList', { ChatUserID: $('#ChatUserID').val() }, function (d) {

            for (var i = 0; i < d.length; i++) {
                var link = $('.user-chat[uid="' + d[i].uid + '"]');
                var def = link.attr('def');
                if (d[i].count > 0) {
                    def += ' <b>(' + d[i].count + ')</b>';
                }
                def += '<span title="Удалить" class="user-del" onclick="deleteUser('+d[i].uid+')">X</span>';
                link.html(def);
                link.attr('count', d[i].count);
            }


            $('.user-chat[uid="' + $('#FromUserID').val() + '"]').addClass('active-link');

            var sorted = $('.user-chat').parent();
            sorted.sort(function(a, b) {
                var compA = parseInt($(a).find('.user-chat').attr('count'));
                var compB = parseInt($(b).find('.user-chat').attr('count'));
                return (compA > compB) ? -1 : (compA < compB) ? 1 : 0;
            });

            

            $('#chatList ul li').not('.skip').remove();
            
            $(sorted).insertAfter('#chatList ul li.skip:first-child');

            rt = setTimeout('refreshChat();', 3000);
        });

    }
}

var rtc = null;
function refreshConnects() {
    
    if ($('#FromUserID').val() && $('#FromUserID').val().length) {
        $.get('/admin/Chat/GetConnectsList', { ChatUserID: $('#FromUserID').val(), count: $('.messages-item').length }, function (d) {

            
            $('#ConnectsWrapper').jscroll.destroy();
            $('#ConnectsWrapper').html(d);
            $("#ConnectsWrapper").jscroll({
                loadingHtml: '<em>Загрузка...</em>',
                nextSelector: "a.next:last"
            });
            
            rtc = setTimeout('refreshConnects();', 3000);
        });

    } else {
        
        rtc = setTimeout('refreshConnects();', 3000);
    }
}

var common;
$(function () {
    refreshChat();
    refreshConnects();
    common = new Common();
    common.init();
});
