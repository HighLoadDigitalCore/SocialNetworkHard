function Money()
{
    var _this = this;

    this.init = function () {

    }

    this.showPopup = function () {
        $.ajax({
            type: "GET",
            url: "/Money/Popup",
            success: function (data) {
                $("#PopupWrapper").html(data);
            }
        })
    }
}

var money = null;
$().ready(function () {
    money = new Money();
    money.init();
})