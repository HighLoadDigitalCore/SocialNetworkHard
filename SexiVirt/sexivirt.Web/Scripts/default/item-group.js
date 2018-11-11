function ItemGroup() {
    var _this = this;

    function loadUsers() {
        $("#LoadAllUsers").click(function () {

            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Group/LoadUsers",
                data: { id: id },
                success: function (data) {
                    $("#GroupUserWrapper").html(data);
                    hideUsers();
                }
            });
            return false;
        });

    }

    function hideUsers() {
        $('#HideAllUsers').click(function () {
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "/Group/HideUsers",
                data: { id: id },
                success: function (data) {
                    $("#GroupUserWrapper").html(data);
                    loadUsers();
                }
            });
            return false;
        });
    }

    this.init = function () {
        loadUsers();

        $("#GroupPostListWrapper").jscroll({
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            callback: function () {
                $("#GroupPostListWrapper").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
            }
        });

        $(document).on("click", "#CreatePostButton", function () {
            $.ajax({
                type: "POST",
                url: "/Group/CreatePost",
                data: $("#CreatePostForm").serialize(),
                success: function (data) {
                    $("#CreatePostWrapper").html(data);
                }
            });
            return false;
        });



        $(document).on("click", ".remove", function () {
            $("#Attach").val("");
            $("#AttachWrapper").empty();
        });


        $("#AttachImageBtn").fineUploader({
            element: $('#AttachImageBtn'),
            request: {
                endpoint: "/File/UploadFile"
            },
            template: 'qq-template-bootstrap',
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
            classes: {
                success: 'alert alert-success',
                fail: 'alert alert-error'
            },
            failedUploadTextDisplay: {
                mode: 'custom',
                maxChars: 400,
                responseProperty: 'error',
                enableTooltip: true
            },
            debug: true
        })
        .on('complete', function (event, id, filename, responseJSON) {
            if (responseJSON.success) {
                $("#Attach").val(responseJSON.fileUrl);
                var obj = $("<img>").attr("src", responseJSON.fileUrl + "?w=300&h=200&mode=crop");
                var deleteDiv = $("<div>").addClass("remove");
                $("#AttachWrapper").html(obj);
                $("#AttachWrapper").append(deleteDiv);
            }
        }).on('submit', function () {
            $(".qq-upload-fail").remove();
            return true;
        });
    }
}

var itemGroup = null;
$().ready(function () {
    itemGroup = new ItemGroup();
    itemGroup.init();
})