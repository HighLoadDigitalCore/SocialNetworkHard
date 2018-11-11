function Meeting() {
    var _this = this;

    this.init = function () {
        $("#SearchBtn").click(function() {
/*
            debugger;
            $(".selectEmul").each(function() {
                var selected = $(this).find('li.selected');
                $(this).find('select').val(selected.text());
            });
*/
            $("#SearchMeetingForm").submit();
        });
        $(".selectEmul").styler({
            selectSmartPositioning: false,
            selectSearch: false,
            selectVisibleOptions: 6,
            singleSelectzIndex: 2,

           /* onSelectClosed: function (data) {
                debugger;
            }*/
        });

        $(".checkboxEmul").styler({
            selectSmartPositioning: false
        });

        $(".radioEmul.gender").click(function () {
            $(".radioEmul.gender").removeClass('active');
            $(this).addClass('active');
            $("#Sex").val($(this).data("value"));
        });

        $(".datePicker").datepicker({
            dateFormat: "dd.mm.yy",
            monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"]
        });


        _this.updateSelect();

        $("#AgeStart").change(function () {
            _this.updateSelect();
        });

        $("#AgeEnd").change(function () {
            _this.updateSelect();
        });

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

        $(document).on("click", "#ChangePreferences", function () {
            $.ajax({
                type: "POST",
                data: $("#SearchMeetingForm").serialize(),
                url: "/People/AjaxPreferences",
                success: function (data) {
                    $("#PopupWrapper").html(data);
                    $("#PopupWrapper").find('.preferences').mCustomScrollbar();
                    $("#PopupWrapper").find('.preferences').mCustomScrollbar('update');
                }
            });
        });


        $(document).on("click", ".preferences .choise li", function () {
            $(this).toggleClass("active");
        });

        $(document).on("click", "#SelectPreferencesBtn", function () {
            var data = [];
            $(".preferences .choise li.active").each(function () {
                data.push($(this).data("id"));
            });

            $.ajax({
                type: "POST",
                traditional: true,
                url: "/People/SearchPreference",
                data: { preferences: data },
                success: function (data) {
                    $("#PreferenceSelectWrapper").html(data);
                    $(".close-popup").click();
                }
            });
        });



        $(document).on("click", "#SelectPreferencesBtn", function () {
            var data = [];
            $(".preferences .choise li.active").each(function () {
                data.push($(this).data("id"));
            });

            $.ajax({
                type: "POST",
                traditional: true,
                url: "/People/SearchPreference",
                data: { preferences: data },
                success: function (data) {
                    $("#PreferenceSelectWrapper").html(data);
                    $(".close-popup").click();
                }
            });
        });

        $(document).on("click", "#ClearPreferences", function () {
            var data = [];

            $.ajax({
                type: "POST",
                traditional: true,
                url: "/People/SearchPreference",
                data: { preferences: data },
                success: function (data) {
                    $("#PreferenceSelectWrapper").html(data);
                }
            });
        });

        $(document).on("click", ".delete-preference", function () {
            var data = [];
            $('#PreferenceSelectWrapper span[data-id="' + $(this).data('id') + '"]').remove();
            $("#PreferenceSelectWrapper span").not('.delete-preference').each(function () {
                data.push($(this).data("id"));
            });

            $.ajax({
                type: "POST",
                traditional: true,
                url: "/People/SearchPreference",
                data: { preferences: data },
                success: function (data) {
                    $("#PreferenceSelectWrapper").html(data);
                }
            });
        });


        $(document).on("click", ".dots", function () {
            var parent = $(this).closest(".cam_services");
            var id = $(this).data("id");
            var opened = $(this).data("opened");

            $.ajax({
                type: "GET",
                url: "/People/UserPreference",
                data: { id: id, opened: opened },
                success: function (data) {
                    parent.html(data);
                }
            });
        });

        $("#MeetingResult").jscroll({
            debug: true,
            loadingHtml: '<em>Загрузка...</em>',
            nextSelector: "a.next:last",
            getParameters: function () {
                return $("#SearchMeetingForm").serialize() + "&skip=" + $("#MeetingResult").data("skip");
            },
            callback: function () {
                $("#MeetingResult").data("skip", $("#LastSkip").val());
                $("#LastSkip").remove();
            }
        });

        $(document).on("click", ".popupMeeting", function () {
            var id = $(this).data("id");

            $.ajax({
                type: "GET",
                url: "/Meeting/Popup",
                data: { id: id },
                success: function (data) {
                    $("#PopupWrapper").html(data);
                }
            });
            return false;
        })
    }

    this.updateSelect = function () {
        return;
        var ageStart = parseInt($("#AgeStart").val());
        var ageEnd = parseInt($("#AgeEnd").val());

        var startBegin = ageStart - 5;
        if (startBegin < 18) {
            startBegin = 18;
        }
        var startEnd = startBegin + 10;
        if (startEnd > ageEnd) {
            startEnd = ageEnd;
        }
        var endBegin = ageEnd - 5;
        if (endBegin < ageStart) {
            endBegin = ageStart;
        }
        var endEnd = endBegin + 10;
        if (endEnd > 75) {
            endEnd = 75;
        }
        $("#AgeStart").empty();
        for (var i = startBegin; i <= startEnd; i++) {
            if (i == ageStart) {
                $("#AgeStart").append("<option value='" + i + "' selected>" + i + "</option>");
            } else {
                $("#AgeStart").append("<option value='" + i + "'>" + i + "</option>");
            }
        }
        $("#AgeEnd").empty();
        for (var i = endBegin; i <= endEnd; i++) {
            if (i == ageEnd) {
                $("#AgeEnd").append("<option value='" + i + "' selected>" + i + "</option>");
            } else {
                $("#AgeEnd").append("<option value='" + i + "'>" + i + "</option>");
            }
        }


    }

}

var meeting = null;
$().ready(function () {
    meeting = new Meeting();
    meeting.init();
});