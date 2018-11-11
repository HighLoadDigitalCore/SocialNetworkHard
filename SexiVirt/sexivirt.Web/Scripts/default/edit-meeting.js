function EditMeeting() {
    var _this = this;

    this.init = function () {
        $(".selectEmul").styler({
            selectSmartPositioning: false,
            selectSearch: false,
        });
        $("#Text").keyup(function () {
            _this.updateLimit();
        })
        _this.updateLimit();

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

        $("#SubmitMeetingBtn").click(function() {
            $("#MeetingForm").submit();
        });
    }

    this.updateLimit = function () {
        var count = $("#Text").val().length;
        var limit = 140 - count;

        if (limit >= 0) {
            $("#Limit").removeClass("exceed");
            $("#Limit").text("Осталось " + limit + " " + _this.countWord(limit, "символ", "символа", "символов"));
            $("#SubmitMeetingBtn").removeAttr("disabled");
        } else {
            $("#Limit").addClass("exceed");
            $("#Limit").text("Превышен размер");
            $("#SubmitMeetingBtn").attr("disabled", "disabled");
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

}

var editMeeting = null;
$().ready(function ()
{
    editMeeting = new EditMeeting();
    editMeeting.init();
});