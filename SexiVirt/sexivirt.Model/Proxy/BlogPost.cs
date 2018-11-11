using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class BlogPost
    {
        public int CommentsCount
        {
            get
            {
                return CommentBlogPosts.Count;
            }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return CommentBlogPosts
                    .Where(p => p.Comment.ParentID == null)
                    .OrderBy(p => p.Comment.AddedDate)
                    .Select(p => p.Comment);
            }
        }
	}
}