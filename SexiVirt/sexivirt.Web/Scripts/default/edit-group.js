function EditGroup()
{
    var _this = this;

    this.init = function () {

        $("#ChangeAvatar").fineUploader({
            element: $('#ChangeAvatar'),
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
                $("#AvatarWrapper").removeClass("hidden");
                $("#ChangeAvatar").addClass("hidden");
                $("#AvatarUrl").val(responseJSON.fileUrl);
                $("#AvatarImage").attr("src", responseJSON.fileUrl + "?w=255&h=255&mode=crop");
            }
        }).on('submit', function () {
            $(".qq-upload-fail").remove();
            return true;
        });


        $("#RemoveAvatar").click(function () {
            $("#AvatarWrapper").addClass("hidden");
            $("#ChangeAvatar").removeClass("hidden");
            $("#AvatarUrl").val("");
            return false;
        });
       
    }
}

var editGroup = null;
$().ready(function () {
    editGroup = new EditGroup();
    editGroup.init();
});
