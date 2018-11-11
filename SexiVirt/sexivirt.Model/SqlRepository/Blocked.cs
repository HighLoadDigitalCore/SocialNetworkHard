using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Blocked> Blockeds
        {
            get
            {
                return Db.Blockeds;
            }
        }

        public bool BlockUser(int senderId, int receverId)
        {
            RemoveFriend(senderId, receverId);
            if (!Db.Blockeds.Any(p => 
                p.SenderID == senderId 
                && p.ReceiverID == receverId))
            {
                var blocked = new Blocked()
                {
                    ReceiverID = receverId,
                    SenderID = senderId,
                };

                Db.Blockeds.InsertOnSubmit(blocked);
                Db.Blockeds.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UnblockUser(int senderId, int receverId)
        {
            var instance = Db.Blockeds.FirstOrDefault(p => p.SenderID == senderId && p.ReceiverID == receverId);

            if (instance != null)
            {
                Db.Blockeds.DeleteOnSubmit(instance);
                Db.Blockeds.Context.SubmitChanges();
                return true;
            }
             return false;
        }
    }
}