function Register() {
    var _this = this;

    this.init = function () {
        $(document).on("click", "#RegisterButton", function () {
            $.ajax({
                type: "POST",
                url: "/User/Register",
                data: $("#RegisterForm").serialize(),
                success: function (data)
                {
                    $("#RegisterWrapper").html(data);
                    $(".selectEmul").styler({
                        selectSearch: false,
                        onSelectOpened: function () {
                            $(this).find('.jq-selectbox__dropdown').mCustomScrollbar("update");

                        }
                    });

                    setTimeout(function () {
                        $('.selectEmul .jq-selectbox__dropdown').mCustomScrollbar({
                            advanced: {
                                updateOnContentResize: false,
                                updateOnBrowserResize: false
                            }
                        });
                    }, 500);
                }
            })
            return false;
        });
    }
}

var register = null;
$().ready(function ()
{
    register = new Register();
    register.init();
});