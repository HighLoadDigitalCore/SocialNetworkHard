function EditGift() {
    var _this = this;

    this.init = function () {
        $("#ChangeImage").fineUploader({
            element: $('#ChangeImage'),
            request: {
                endpoint: "/File/UploadGift"
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
                $("#PreviewImage").attr("src", responseJSON.fileUrl+"?w=256&h=256&mode=crop&scale=both");
                $("#Image").val(responseJSON.fileUrl);
            }
        }).on('submit', function () {
            $(".qq-upload-fail").remove();
            return true;
        });
    };
}

var editGift;
$(function () {
    editGift = new EditGift();
    editGift.init();
});
