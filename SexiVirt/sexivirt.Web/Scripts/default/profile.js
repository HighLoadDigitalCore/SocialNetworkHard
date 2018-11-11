function Profile() {
    var _this = this;

    this.init = function () {

        $(document).on("click", "#StatusActiveBtn", function () {
            $.ajax({
                type: "GET",
                url: "/Home/StatusForm",
                success: function (data) {
                    $("#StatusWrapper").html(data);
                    _this.initStatus();
                    _this.updateLimit();
                }
            })
        });

        $(document).on("click", "#StatusChangeBtn", function () {
            if ($(this).attr("disabled") === undefined) {
                $.ajax({
                    type: "POST",
                    url: "/Home/StatusForm",
                    data: { status: $("#StatusTextArea").val() },
                    success: function (data) {
                        $("#StatusWrapper").html(data);
                    }
                });
            };
        });

        $(document).on("click", "#EditInfoActive", function () {
            $.ajax({
                type: "GET",
                url: "/Home/EditUserInfo",
                success: function (data) {
                    $("#UserInfoWrapper").html(data);
                    _this.initUserInfo();
                    $("#UserInfoWrapper .selectEmul").styler({
                        selectSearch: false,
                        onSelectOpened: function () {
                            $(this).find('.jq-selectbox__dropdown').mCustomScrollbar("update");

                        }
                    });

                    setTimeout(function () {
                        $('#UserInfoWrapper .selectEmul .jq-selectbox__dropdown').mCustomScrollbar({
                            advanced: {
                                updateOnContentResize: false,
                                updateOnBrowserResize: false
                            }
                        });
                    }, 500);

                }
            });
            return false;
        });

        $(document).on("click", "#SaveUserInfoBtn", function () {
            $.ajax({
                type: "POST",
                url: "/Home/EditUserInfo",
                data: $("#EditUserInfoForm").serialize(),
                success: function (data) {
                    $("#UserInfoWrapper").html(data);
                    if ($("#SaveUserInfoBtn").length > 0) {
                        _this.initUserInfo();
                    }

                }
            })
            return false;
        })

        $(document).on("click", "#DescriptionChangeActive", function () {
            $.ajax({
                type: "GET",
                url: "/Home/EditUserDescription",
                success: function (data) {
                    $("#UserDescriptionWrapper").html(data);
                }
            });
            return false;
        });

        $(document).on("click", "#DescriptionChangeBtn", function () {
            $.ajax({
                type: "POST",
                url: "/Home/EditUserDescription",
                data: $("#EditUserDescForm").serialize(),
                success: function (data) {
                    $("#UserDescriptionWrapper").html(data);
                }
            })
        });

        $("#ChangeAvatar").fineUploader({
            element: $('#ChangeAvatar'),
            request: {
                endpoint: "/File/UploadAvatar"
            },
            template: 'qq-template-bootstrap',
            allowedExtensions: ['jpg', 'jpeg', 'png']
        }).on('complete', function (event, id, filename, responseJSON) {
            if (responseJSON.success) {
                $("#MenuAvatar").attr("src", responseJSON.fileUrl + "?w=94&h=94&mode=crop");
                $("#ProfileAvatar").attr("src", responseJSON.fileUrl + "?w=183&h=183&mode=crop");
            }
        });

        $(document).on("click", "#ChangePreferences", function () {
            $.ajax({
                type: "GET",
                url: "/Home/AjaxPreferences",
                success: function (data) {
                    $("#PopupWrapper").html(data);
                    $("#PopupWrapper").find('.preferences').mCustomScrollbar();
                    $("#PopupWrapper").find('.preferences').mCustomScrollbar('update');
                }
            });
        });

        $(document).on("click", ".preferences .choise li", function () {
            var id = $(this).data("id");
            var item = $(this);
            $.ajax({
                type: "GET",
                url: "/Home/TogglePreference",
                data: { id: id },
                success: function (data) {
                    if (data.result == "ok") {
                        item.toggleClass("active");
                    }
                }
            });
        });

        $(document).on("click", ".closePreference", function () {
            $.ajax({
                type: "GET",
                url: "/Home/UserPreferences",
                success: function (data) {
                    $("#PreferencesWrapper").html(data);
                }
            });
        });
    }

    this.initStatus = function () {
        $("#StatusTextArea").keyup(function () {
            _this.updateLimit();
        })
    }

    this.updateLimit = function () {
        var count = $("#StatusTextArea").val().length;
        var limit = 140 - count;

        if (limit >= 0) {
            $("#Limit").removeClass("exceed");
            $("#Limit").text("Еще " + limit + " " + _this.countWord(limit, "символ", "символа", "символов"));
            $("#StatusChange").removeAttr("disabled");
        } else {
            $("#Limit").addClass("exceed");
            $("#Limit").text("Превышен размер");
            $("#StatusChange").attr("disabled", "disabled");
        }

    }

    this.countWord = function (count, first, second, five) {
        if (count % 10 == 1 && (count / 10) != 1) {
            return first;
        }
        if (count % 10 > 1 && count % 10 < 5 && ((count / 10) % 10) != 1) {
            return second;
        }
        return five;
    }

    this.initUserInfo = function () {
        $("#CityName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Home/SelectCity",
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
                $("#CityName").val(ui.item.label);
                $("#CityID").val(ui.item.id);
                return false;
            }
        });
    }
}

var profile = null;
$().ready(function () {
    profile = new Profile();
    profile.init();
});