function AsideAuth() {
    var _this = this;

    this.init = function () {

        $(document).on("click", "#AjaxLoginBtn", function () {
            $.ajax({
                type: "POST",
                url : "/Login/AjaxAside",
                data: $("#AjaxAsideLoginForm").serialize(),
                success: function (data)
                {
                    $("#AsideLoginWrapper").html(data);
                }
            });
            return false;
        });
    }
}
var asideAuth = null;
$().ready(function () {
    asideAuth = new AsideAuth();
    asideAuth.init();
})