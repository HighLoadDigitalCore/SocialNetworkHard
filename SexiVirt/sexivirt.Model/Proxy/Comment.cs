using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Comment
    {
        public IEnumerable<Comment> SubComments
        {
            get
            {
                return Comments.OrderBy(p => p.AddedDate).ToList();
            }
        }

        public int CalculatedRating
        {
            get { return CommentRatings.Sum(x => x.Mark); }
        }

        public User EnityOwner
        {
            get
            {
                if (CommentBlogPosts.Any())
                {
                    return CommentBlogPosts.First().BlogPost.User;
                }
                if (CommentEvents.Any())
                {
                    return CommentEvents.First().Event.User;
                }
                if (CommentGroupBlogPosts.Any())
                {
                    return CommentGroupBlogPosts.First().GroupBlogPost.Group.User;
                }
                return null;
            }
        }
	}
}