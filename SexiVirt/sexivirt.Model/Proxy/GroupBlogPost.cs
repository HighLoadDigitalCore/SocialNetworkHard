using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class GroupBlogPost
    {

        public int CommentsCount
        {
            get
            {
                return CommentGroupBlogPosts.Count;
            }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return CommentGroupBlogPosts.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate).AsEnumerable();
            }
        }
		
	}
}