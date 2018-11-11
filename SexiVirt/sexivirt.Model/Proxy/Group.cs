using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Group
    {

        public int SubUsersCount
        {
            get
            {
                return UserGroups.Count;
            }
        }

        public IEnumerable<User> SubUsers
        {
            get
            {
                return UserGroups.Select(p => p.User).AsEnumerable();
            }
        }

        public IQueryable<GroupBlogPost> SubBlogPost(int skip)
        {
                return GroupBlogPosts.OrderByDescending(p => p.AddedDate).Skip(skip).AsQueryable();
        }

        public int SubBlogPostCount
        {
            get
            {
                return GroupBlogPosts.Count;
            }
        }
	}
}