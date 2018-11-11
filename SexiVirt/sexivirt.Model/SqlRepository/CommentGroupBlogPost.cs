using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<CommentGroupBlogPost> CommentGroupBlogPosts
        {
            get
            {
                return Db.CommentGroupBlogPosts;
            }
        }

        public bool CreateCommentGroupBlogPost(CommentGroupBlogPost instance)
        {
            if (instance.ID == 0)
            {
                Db.CommentGroupBlogPosts.InsertOnSubmit(instance);
                Db.CommentGroupBlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCommentGroupBlogPost(CommentGroupBlogPost instance)
        {
            var cache = Db.CommentGroupBlogPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.CommentID = instance.CommentID;
				cache.GroupBlogPostID = instance.GroupBlogPostID;
                Db.CommentGroupBlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCommentGroupBlogPost(int idCommentGroupBlogPost, int? userID = null)
        {
            CommentGroupBlogPost instance = Db.CommentGroupBlogPosts.FirstOrDefault(p => p.ID == idCommentGroupBlogPost);
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
                    Db.CommentGroupBlogPosts.DeleteOnSubmit(instance);
                    Db.CommentGroupBlogPosts.Context.SubmitChanges();
                    return true;

                }
                return true;
            }
            return false;
        }
    }
}