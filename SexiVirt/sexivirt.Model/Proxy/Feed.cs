using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Feed
    {
        public enum ActionTypeEnum
        {
            AddFriend = 0x01,
            AddBlogPostComment = 0x02,
            AddGroupBlogPostComment = 0x03,
            AddEventComment = 0x04, 
            PayForAlbumAccess = 0x05,
            AddCommentToComment = 0x06
        }

        public User Owner
        {
            get
            {
                return User1;
            }
            set
            {
                User1 = value;
            }
        }

        public User Actor
        {
            get
            {
                return User;
            }
            set
            {
                User = value;
            }
        }
	}
}