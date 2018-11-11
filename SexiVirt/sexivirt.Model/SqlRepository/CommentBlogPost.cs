using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{

    public partial class SqlRepository
    {
        public IQueryable<CommentBlogPost> CommentBlogPosts
        {
            get
            {
                return Db.CommentBlogPosts;
            }
        }

        public bool CreateCommentBlogPost(CommentBlogPost instance)
        {
            if (instance.ID == 0)
            {
                Db.CommentBlogPosts.InsertOnSubmit(instance);
                Db.CommentBlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCommentBlogPost(CommentBlogPost instance)
        {
            var cache = Db.CommentBlogPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.CommentID = instance.CommentID;
                cache.BlogPostID = instance.BlogPostID;
                Db.CommentBlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCommentBlogPost(int idCommentBlogPost, int? userID = null)
        {
            CommentBlogPost instance = Db.CommentBlogPosts.FirstOrDefault(p => p.ID == idCommentBlogPost);
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
                    Db.CommentBlogPosts.DeleteOnSubmit(instance);
                    Db.CommentBlogPosts.Context.SubmitChanges();
                    return true;
                }
            }
            return false;
        }
    }
}