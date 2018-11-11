using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserGift> UserGifts
        {
            get
            {
                return Db.UserGifts;
            }
        }

        public bool CreateUserGift(UserGift instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.UserGifts.InsertOnSubmit(instance);
                Db.UserGifts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserGift(UserGift instance)
        {
            var cache = Db.UserGifts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.GiftID = instance.GiftID;
				cache.SenderID = instance.SenderID;
				cache.ReceiverID = instance.ReceiverID;
				cache.AddedDate = instance.AddedDate;
				cache.Text = instance.Text;
				cache.Visible = instance.Visible;
                Db.UserGifts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserGift(int idUserGift)
        {
            UserGift instance = Db.UserGifts.FirstOrDefault(p => p.ID == idUserGift);
            if (instance != null)
            {
                Db.UserGifts.DeleteOnSubmit(instance);
                Db.UserGifts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}