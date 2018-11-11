using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Group> Groups
        {
            get
            {
                return Db.Groups;
            }
        }

        public bool CreateGroup(Group instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Groups.InsertOnSubmit(instance);
                Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGroup(Group instance)
        {
            var cache = Db.Groups.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Info = instance.Info;
				cache.IsBanned = instance.IsBanned;
				cache.IsVip = instance.IsVip;
				cache.AvatarUrl = instance.AvatarUrl;
                Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGroup(int idGroup)
        {
            Group instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (instance != null)
            {
                if (instance.GroupBlogPosts.Any())
                {
                    foreach (var blogPost in instance.GroupBlogPosts)
                    {
                        if (blogPost.Feeds.Any())
                        {
                            
                            Db.Feeds.DeleteAllOnSubmit(blogPost.Feeds);
                            Db.Feeds.Context.SubmitChanges();
                        }
                        if (blogPost.CommentGroupBlogPosts.Any())
                        {
                            Db.CommentGroupBlogPosts.DeleteAllOnSubmit(blogPost.CommentGroupBlogPosts);
                            Db.CommentGroupBlogPosts.Context.SubmitChanges();
                        }
                        if (blogPost.SubComments.Any())
                        {
                            Db.Comments.DeleteAllOnSubmit(blogPost.SubComments);
                            Db.Comments.Context.SubmitChanges();
                        }
                    }
                    Db.GroupBlogPosts.DeleteAllOnSubmit(instance.GroupBlogPosts);
                    Db.GroupBlogPosts.Context.SubmitChanges();
                }
                if (instance.UserGroups.Any())
                {
                    Db.UserGroups.DeleteAllOnSubmit(instance.UserGroups);
                    Db.UserGroups.Context.SubmitChanges();
                }



                Db.Groups.DeleteOnSubmit(instance);
                Db.Groups.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}