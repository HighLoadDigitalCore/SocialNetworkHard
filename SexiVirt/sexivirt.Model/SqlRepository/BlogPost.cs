using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BlogPost> BlogPosts
        {
            get
            {
                return Db.BlogPosts;
            }
        }

        public bool CreateBlogPost(BlogPost instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.BlogPosts.InsertOnSubmit(instance);
                Db.BlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBlogPost(BlogPost instance)
        {
            var cache = Db.BlogPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Header = instance.Header;
				cache.Text = instance.Text;
				cache.Attach = instance.Attach;
                Db.BlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBlogPost(int idBlogPost)
        {
            BlogPost instance = Db.BlogPosts.FirstOrDefault(p => p.ID == idBlogPost);
            if (instance != null)
            {
                var feeds = instance.Feeds.ToList();
                Db.Feeds.DeleteAllOnSubmit(feeds);
                Db.Feeds.Context.SubmitChanges();
                
                Db.BlogPosts.DeleteOnSubmit(instance);
                Db.BlogPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}