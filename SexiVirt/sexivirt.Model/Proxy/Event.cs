using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Event
    {
        public IEnumerable<User> SubUsers
        {
            get
            {
                return UserEvents.Select(p => p.User).AsEnumerable();
            }
        }

        public int SubUsersCount
        {
            get
            {
                return UserEvents.Count;
            }
        }

        public int CommentsCount
        {
            get
            {
                return CommentEvents.Count;
            }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return CommentEvents
                    .Where(p => p.Comment.ParentID == null)
                    .OrderBy(p => p.Comment.AddedDate)
                    .Select(p => p.Comment);
            }
        }
	}
}