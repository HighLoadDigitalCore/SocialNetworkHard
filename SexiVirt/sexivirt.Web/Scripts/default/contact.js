function Contact() {
    var _this = this;

    var ajaxNewRequest = null;
    var ajaxFullRequst = null;
    this.init = function () {

        $(document).on("click", ".confirmFriendship", function () {
            var id = $(this).data("id");
            var item = $(this);
            $.ajax({
                type: "GET",
                url: "/Friend/ConfirmFriendship",
                data: { id: id },
                success: function (data) {
                    document.location.reload();
/*
                    _this.addContact(id);
                    item.closest(".contact-item").remove();
*/
                }
            });
        });

        $(document).on("click", ".declineFriendship", function () {
            var id = $(this).data("id");
            var item = $(this);
            $.ajax({
                type: "GET",
                url: "/Friend/DeclineFriendship",
                data: { id: id },
                success: function (data) {
                    item.closest(".contact-item").remove();
                }
            });
        });

        $(document).on("click", ".removeFriend", function () {
            var id = $(this).data("id");
            var item = $(this);
            $.ajax({
                type: "GET",
                url: "/Friend/RemoveFriend",
                data: { id: id },
                success: function (data) {
                    item.closest(".contact-item").remove();
                }
            });
        });
        
        $(document).on("click", ".blockUser", function () {
            var id = $(this).data("id");
            var item = $(this);
            $.ajax({
                type: "GET",
                url: "/Friend/BlockUser",
                data: { id: id },
                success: function (data) {
                    item.closest(".contact-item").remove();
                }
            });
        });

        $("#MyContactsWrapper").jscroll({
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            getParameters: function () {
                return $("#ContactSearchForm").serialize() + "&skip=" + $("#MyContactsWrapper").data("skip");
            },
            callback: function () {
                $("#MyContactsWrapper").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
            }
        });

        var count = $("#MyContactsWrapper .contact-item").length;
        $("#MyContactsWrapper").data("skip", count);

        $("#SearchString").donetyping(function ()
        {
            var data = $("#SearchString").val();
            
            _this.searchNew(data);
            _this.searchFull(data);

        }, 500);
    }

    this.searchNew = function (searchString) {
        ajaxNewRequest = $.ajax({
            type: "GET",
            url: "/Friend/NewContacts",
            data: { SearchString: searchString },
            success: function (data) {
                $("#NewContactsWrapper").html(data);
            }
        })
    }

    this.searchFull = function (searchString) {
        ajaxFullRequest= $.ajax({
            type: "GET",
            url: "/Friend/MyContacts",
            data: { SearchString: searchString },
            success: function (data) {
                $("#MyContactsWrapper").html(data);
                var count = $("#MyContactsWrapper .contact-item").length;
                $("#MyContactsWrapper").data("skip", count);
            }
        })
    }

    this.addContact = function (id) {
        $.ajax({
            type: "GET",
            url: "/Friend/AddContact",
            data: { id: id },
            success: function (data) {
                $("#MyContactsWrapper .jscroll-inner").prepend(data);
            }
        })

    }
}

var contact = null;
$().ready(function () {
    contact = new Contact();
    contact.init();
});