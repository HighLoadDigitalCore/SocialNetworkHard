using sexivirt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class CommentController : DefaultController
    {
        public ActionResult Rating(int id, bool mark)
        {
            var comment = Repository.Comments.FirstOrDefault(p => p.ID == id);

            if (CurrentUser != null && comment != null)
            {
                var entry = Repository.CommentRatings.FirstOrDefault(p => p.CommentID == id && p.UserID == CurrentUser.ID);
                if (entry != null)
                {
                    if ((entry.Mark == 1 && mark) || (entry.Mark == -1 && !mark))
                    {
                        return PartialView(comment);
                    }
                    else
                    {
                        Repository.RemoveCommentRating(entry.ID, CurrentUser.ID);
                    }
                }
                else
                {
                    Repository.CreateCommentRating(new CommentRating
                    {
                        UserID = CurrentUser.ID,
                        CommentID = comment.ID,
                        Mark = mark ? 1 : -1
                    });
                }
            }
            var resultComment = Repository.Comments.FirstOrDefault(p => p.ID == id);
            return PartialView(resultComment);
        }

        public ActionResult DeleteComment(int id)
        {
            if (CurrentUser != null)
            {
                var comment = Repository.Comments.FirstOrDefault(p => p.ID == id);
                if (comment != null && (CurrentUser.ID == comment.UserID
                    || comment.EnityOwner != null && comment.EnityOwner.ID == CurrentUser.ID))
                {
                    Repository.RemoveComment(id, CurrentUser.ID);
                }
            }
            return Json(new { result = "ok" });
        }
	}
}