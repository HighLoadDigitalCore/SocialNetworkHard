using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Message> Messages
        {
            get
            {
                return Db.Messages;
            }
        }

        public bool CreateMessage(Message instance)
        {
            if (instance.ID == 0)
            {
                var connect = Db.Connects.FirstOrDefault(p => (p.UserID == instance.SenderID && p.OtherUserID == instance.ReceiverID)
                    ||
                    (p.UserID == instance.ReceiverID && p.OtherUserID == instance.SenderID));

                if (connect != null)
                {
                    instance.AddedDate = DateTime.Now;
                    instance.ConnectID = connect.ID;
                    var cache = new Message()
                    {
                        IsSend = true,
                        ReceiverID = instance.ReceiverID,
                        SenderID = instance.SenderID,
                        Text = instance.Text,
                        AddedDate = instance.AddedDate,
                        ConnectID = instance.ConnectID,
                        ReadedDate = null,
                    };
                    Db.Messages.InsertOnSubmit(instance);
                    Db.Messages.InsertOnSubmit(cache);
                    Db.Messages.Context.SubmitChanges();
                    return true;
                }
            }

            return false;
        }

        public bool ReadMessages(int idReader, int idConnect)
        {
            var reader = Db.Users.FirstOrDefault(p => p.ID == idReader);
            if (reader != null)
            {
                var messages = Db.Messages.Where(p => p.ReceiverID == idReader && p.ConnectID == idConnect && !p.ReadedDate.HasValue).ToList();
                foreach (var message in messages)
                {
                    message.ReadedDate = DateTime.Now;
                    Db.Messages.Context.SubmitChanges();
                }
                return true;
            }
            return false;
        }

        public bool RemoveAllMessages(int idReader, int idConnect)
        {
            var reader = Db.Users.FirstOrDefault(p => p.ID == idReader);
            if (reader != null)
            {
                var messages = Db.Messages.Where(p => p.ReceiverID == idReader && p.ConnectID == idConnect && !p.IsSend).ToList();
                Db.Messages.DeleteAllOnSubmit(messages);
                messages = Db.Messages.Where(p => p.SenderID == idReader && p.ConnectID == idConnect && p.IsSend).ToList();
                Db.Messages.DeleteAllOnSubmit(messages);

                Db.Messages.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveMessage(int idMessage)
        {
            var instance = Db.Messages.Where(p => p.ID == idMessage).FirstOrDefault();
            if (instance != null)
            {
                Db.Messages.DeleteOnSubmit(instance);
                Db.Messages.Context.SubmitChanges();
                return true;
            }

            return false;
        }
    }
}