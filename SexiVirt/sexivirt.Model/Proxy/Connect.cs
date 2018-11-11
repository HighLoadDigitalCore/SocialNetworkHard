using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{
    public partial class Connect
    {
        public User Viral(User user)
        {
            if (User.ID == user.ID)
            {
                return User1;
            }
            return User;
        }

        public int UnreadMessagesCount(int receiverID)
        {
            return Messages.Count(p => p.ReceiverID == receiverID && !p.ReadedDate.HasValue && !p.IsSend);
        }

        public int MessagesCount(int receiverID)
        {
            return Messages.Count(p => (p.ReceiverID == receiverID && !p.IsSend) || (p.SenderID == receiverID && p.IsSend));
        }

        public int TotalMessagesCount
        {
            get
            {
                return Messages.Count(p => !p.IsSend);
            }
        }

        public Message LastMessage(int receiverID)
        {
            var m = Messages.Where(p => (p.ReceiverID == receiverID && !p.IsSend) || (p.SenderID == receiverID && p.IsSend)).OrderByDescending(p => p.ID).FirstOrDefault();
            return m;
        }
    }
}