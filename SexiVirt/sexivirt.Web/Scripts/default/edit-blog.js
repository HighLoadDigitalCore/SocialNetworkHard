var editor;
function EditBlog() {
    var _this = this;

    this.init = function () {
        editor = $("#Text").cleditor({ controls: "bold italic underline strikethrough" })[0];
        $(editor.$frame[0]).attr("id", "cleditCool");

        var cleditFrame;
        if (!document.frames) {
            cleditFrame = $("#cleditCool")[0].contentWindow.document;
        }
        else {
            cleditFrame = document.frames["cleditCool"].document;
        }

        $(cleditFrame).bind('click', function () {
/*
            var v = $(this).text();
            alert(v);
*/
/*            editor.focus();*/
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
            if (responseJSON.success)
            {
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

var editBlog;
$().ready(function () {
    editBlog = new EditBlog();
    editBlog.init();
})