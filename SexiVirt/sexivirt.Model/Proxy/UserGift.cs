using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class UserGift
    {
        public User Sender
        {
            get
            {
                return User1;
            }
        }

        public User Receiver
        {
            get
            {
                return User;
            }
        }
	}
}