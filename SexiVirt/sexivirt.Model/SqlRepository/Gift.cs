using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Gift> Gifts
        {
            get
            {
                return Db.Gifts;
            }
        }

        public bool CreateGift(Gift instance)
        {
            if (instance.ID == 0)
            {
                Db.Gifts.InsertOnSubmit(instance);
                Db.Gifts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGift(Gift instance)
        {
            var cache = Db.Gifts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Image = instance.Image;
				cache.Type = instance.Type;
				cache.Price = instance.Price;
				cache.IsActive = instance.IsActive;
                Db.Gifts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGift(int idGift)
        {
            Gift instance = Db.Gifts.FirstOrDefault(p => p.ID == idGift);
            if (instance != null)
            {
                
                if (instance.UserGifts.Any())
                {
                    Db.UserGifts.DeleteAllOnSubmit(instance.UserGifts);
                    Db.UserGifts.Context.SubmitChanges();
                }

                Db.Gifts.DeleteOnSubmit(instance);
                Db.Gifts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}