function EditAlbum() {
    var _this = this;

    this.init = function () {

        $(".selectEmul,.checkboxEmul").styler({
            selectSmartPositioning: false,
            selectSearch: false
        });

        $("#UploadPhoto").fineUploader({
            element: $('#UploadPhoto'),
            request: {
                endpoint: "/File/UploadPhoto"
            },
            template: 'qq-template-bootstrap',
            allowedExtensions: ['jpg', 'jpeg', 'png']
        }).on('complete', function (event, id, filename, responseJSON) {
            if (responseJSON.success) {
                _this.createPhoto(responseJSON.fileUrl);
            }
        });
        $("#IsPayed-styler").click(function () {
            _this.updateCost();
        });
        $("#Price").blur(function () {
            _this.updateCost();
        })
        _this.updateCost();

        $(document).on("change", ".checkboxEmul", function (event) {
            $(this).closest(".photo-item").toggleClass("selected");
            event.stopPropagation();
        });

        $("#DeletePhoto").click(function () {
            $(".photo-item.selected").remove();
        });

        $("#MakeCover").click(function () {
            $(".CoverInput").val("false");
            $(".photo-item.active").removeClass("active");
            var obj = $(".photo-item.selected").first();
            obj.addClass("active");
            $(".photo-item.selected").first().find(".CoverInput").val("true");
            
            $(".photo-item.selected .jq-checkbox").removeClass("checked");
            $(".photo-item.selected .jq-checkbox").removeAttr("checked");
            $(".photo-item.selected input[type=checkbox]").removeAttr("checked");
            $(".photo-item.selected").removeClass("selected");
        });
    }

    this.createPhoto = function (filePath) {
        $.ajax({
            type: "GET",
            url: "/Album/PhotoItem",
            data: { filePath: filePath },
            success: function (data) {

                $("#AlbumWrapper").append(data);

                $(".checkboxEmul").styler({
                    selectSmartPositioning: false
                });
            }
        })
    }

    this.updateCost = function () {
        if ($("input[name='IsPayed']")[0].checked) {
            $("#CostWrapper").show();
        } else {
            $("#CostWrapper").hide();
        }
        var price = parseInt($("#Price").val());
        if (isNaN(price)) {
            $("#CostInfoWrapper").hide();
        } else {
            var clearPrice = parseInt(price * 0.8);
            $("#PriceWrapper").text(clearPrice);
            $("#Price").removeClass("input-validation-error");
            $("#CostInfoWrapper").show();
        }
     
    }
}

var editAlbum = null;
$().ready(function () {
    editAlbum = new EditAlbum();
    editAlbum.init();
})