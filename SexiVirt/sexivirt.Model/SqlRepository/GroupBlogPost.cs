using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<GroupBlogPost> GroupBlogPosts
        {
            get
            {
                return Db.GroupBlogPosts;
            }
        }

        public bool CreateGroupBlogPost(GroupBlogPost instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.GroupBlogPosts.InsertOnSubmit(instance);
                Db.GroupBlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGroupBlogPost(GroupBlogPost instance)
        {
            var cache = Db.GroupBlogPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Header = instance.Header;
				cache.Text = instance.Text;
				cache.Attach = instance.Attach;
                Db.GroupBlogPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGroupBlogPost(int idGroupBlogPost)
        {
            var instance = Db.GroupBlogPosts.FirstOrDefault(p => p.ID == idGroupBlogPost);
            if (instance != null)
            {
                var feeds = instance.Feeds.ToList();
                Db.Feeds.DeleteAllOnSubmit(feeds);
                
                Db.GroupBlogPosts.DeleteOnSubmit(instance);
                Db.GroupBlogPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}