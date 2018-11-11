using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Comment> Comments
        {
            get
            {
                return Db.Comments;
            }
        }

        public bool CreateComment(Comment instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Comments.InsertOnSubmit(instance);
                Db.Comments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateComment(Comment instance)
        {
            var cache = Db.Comments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.Text = instance.Text;
                Db.Comments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveComment(int idComment, int? userID = null)
        {
            var instance = Db.Comments.FirstOrDefault(p => p.ID == idComment);
            if (instance != null)
            {
                var list = instance.Comments.ToList();
                var feeds = instance.Feeds.ToList();
                if (list.Any() || feeds.Any())
                {
                    instance.IsDeleted = true;
                    instance.DeleteAuthorID = userID;
                    Db.Comments.Context.SubmitChanges();
                    return false;

                }
                else
                {
                    Db.Comments.DeleteOnSubmit(instance);
                    Db.Comments.Context.SubmitChanges();
                    return true;

                }
                /*foreach (var comment in list)
                {
                    RemoveComment(comment.ID);
                }

                
                Db.Feeds.DeleteAllOnSubmit(feeds);
                */
            }
            return false;
        }
    }
}