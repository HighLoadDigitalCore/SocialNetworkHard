using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Message
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