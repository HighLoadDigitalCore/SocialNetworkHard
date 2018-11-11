using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{

    public partial class SqlRepository
    {
        public IQueryable<CommentEvent> CommentEvents
        {
            get
            {
                return Db.CommentEvents;
            }
        }

        public bool CreateCommentEvent(CommentEvent instance)
        {
            if (instance.ID == 0)
            {
                Db.CommentEvents.InsertOnSubmit(instance);
                Db.CommentEvents.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCommentEvent(CommentEvent instance)
        {
            var cache = Db.CommentEvents.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.CommentID = instance.CommentID;
                cache.EventID = instance.EventID;
                Db.CommentEvents.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCommentEvent(int idCommentEvent, int? userID = null)
        {
            CommentEvent instance = Db.CommentEvents.FirstOrDefault(p => p.ID == idCommentEvent);
            if (instance != null)
            {
                var list = instance.Comment.Comments.ToList();
                var feeds = instance.Comment.Feeds.ToList();
                if (list.Any() || feeds.Any())
                {
                    instance.Comment.IsDeleted = true;
                    instance.Comment.DeleteAuthorID = userID;
                    Db.Comments.Context.SubmitChanges();
                    return false;

                }
                else
                {
                    Db.CommentEvents.DeleteOnSubmit(instance);
                    Db.CommentEvents.Context.SubmitChanges();
                    return true;
                }
            }
            return false;
        }
    }
}