function Post() {

    var _this = this;

    this.init = function () {
        $(document).on("click", "#SubmitCommentBtn", function () {

            $.ajax({
                type: "POST",
                url: "/Blog/CreateComment",
                data: $("#CommentForm").serialize(),
                success: function (data)
                {
                    $("#PostCommentWrapper").html(data);
                }
            });
            return false;
        });

        $(document).on("click", ".commentReply", function () {
            
            if ($(this).data('copy')!='') {
                $('#CopyID').val($(this).data('copy'));
            } else {
                $('#CopyID').val('');
            }

            $("#ParentID").val($(this).data("id"));
            $("#AnswerTo").removeClass("hidden");
            $("#AnswerToName").text($(this).data("name"));
            $("html, body").animate({ scrollTop: $(document).height() }, 1000);
        });

        $(document).on("click", ".answer-to .close", function () {
            $('#CopyID').val('');
            $("#ParentID").val("");
            $("#AnswerTo").addClass("hidden");
            $("#AnswerToName").text("");
        });

        $(document).on("click", ".deleteComment", function () {
            var id = $(this).data("id");
            $.ajax({
                type: "POST",
                url: "/Comment/DeleteComment",
                data: {id : id},
                success: function (data) {
                    document.location.reload();
/*
                    $(".commentItem[data-id=" + id + "]").remove();
                    $(".commentItem[data-parent=" + id + "]").remove();
*/
                }
            });
        });

         
        $(document).on("click", ".voteForComment", function () {
            var id = $(this).data("id");
            var mark = $(this).data("mark");
            var wrapper = $(this).closest(".rating-wrapper");
            $.ajax({
                type: "GET",
                url: "/Comment/Rating",
                data: {
                    id: id,
                    mark: mark
                },
                success: function (data) {
                    $(wrapper).html(data);
                }
            })
        });
        debugger;
        var commentId = getURLParameter('commentId');
        if (commentId != undefined) {
            $(".commentReply[data-cid=" + commentId + "]").click();
        };
    };
}

var post = null;
$().ready(function () {
    post = new Post();
    post.init();
});