using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Friendship
    {
        public User Sender
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

        public bool HasConversation
        {
            get { return Sender.Connects.Any(x => x.OtherUserID == ReceiverID || x.UserID == ReceiverID); }
        }

        public User Receiver
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