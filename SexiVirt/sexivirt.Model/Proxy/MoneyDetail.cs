using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class MoneyDetail
    {
        public enum TypeEnum : int
        {
            Income = 0x01,

            GetForAlbum = 0x02,

            Withdraw = 0x10,
            ProAccount = 0x11, 
            PayForAlbum = 0x12,
            PayForGift = 0x13,
            PayForMeeting = 0x14,

            Fee = 0x20,
            PayForMessage  = 0x30
        }
	}
}