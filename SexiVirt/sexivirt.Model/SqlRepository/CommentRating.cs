using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{

    public partial class SqlRepository
    {
        public IQueryable<CommentRating> CommentRatings
        {
            get
            {
                return Db.CommentRatings;
            }
        }

        public bool CreateCommentRating(CommentRating instance)
        {
            if (instance.ID == 0)
            {
                Db.CommentRatings.InsertOnSubmit(instance);
                Db.CommentRatings.Context.SubmitChanges();
                UpdateCommentRating(instance.CommentID);
                return true;
            }

            return false;
        }


        public bool RemoveCommentRating(int idCommentRating, int? userID)
        {
            CommentRating instance = Db.CommentRatings.FirstOrDefault(p => p.ID == idCommentRating);
            if (instance != null)
            {
                var list = instance.Comment.Comments.ToList();
                var feeds = instance.Comment.Feeds.ToList();
                if (list.Any() || feeds.Any())
                {
                    instance.Comment.IsDeleted = true;
                    instance.Comment.DeleteAuthorID = userID;
                    Db.Comments.Context.SubmitChanges();
                    UpdateCommentRating(instance.CommentID);
                    return false;

                }
                else
                {
                    var id = instance.CommentID;
                    Db.CommentRatings.DeleteOnSubmit(instance);
                    Db.CommentRatings.Context.SubmitChanges();
                    UpdateCommentRating(id);
                }
                return true;
            }
            return false;
        }

        private void UpdateCommentRating(int commentID)
        {
            var comment = Db.Comments.FirstOrDefault(p => p.ID == commentID);
            if (comment != null)
            {
                var any = Db.CommentRatings.Any(p => p.CommentID == comment.ID);
                if (any)
                {
                    comment.Rating = Db.CommentRatings.Where(p => p.CommentID == comment.ID && !p.Comment.IsDeleted).Sum(p => p.Mark);
                    Db.CommentRatings.Context.SubmitChanges();
                }
            }
        }

    }
}