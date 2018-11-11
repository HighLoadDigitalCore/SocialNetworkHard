function Auth() {
    var _this = this;

    this.init = function () {
        $("#AuthAjaxButton").unbind('click');
        $(document).on("click", "#AuthAjaxButton", function () {
            $('#AuthAjaxForm').submit();
            return false;
/*
            $.ajax({
                type: "POST",
                url : "/Login/Ajax",
                data: $("#AuthAjaxForm").serialize(),
                success: function (data)
                {
                    $("#AuthWrapper").html(data);
                }
            });
            return false;
*/
        });
    }
}

var auth = null;
$().ready(function () {
    auth = new Auth();
    auth.init();
})