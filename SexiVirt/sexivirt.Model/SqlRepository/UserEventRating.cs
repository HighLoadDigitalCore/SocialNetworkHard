using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserEventRating> UserEventRatings
        {
            get
            {
                return Db.UserEventRatings;
            }
        }

        public bool CreateUserEventRating(UserEventRating instance)
        {
            if (instance.ID == 0)
            {
                Db.UserEventRatings.InsertOnSubmit(instance);
                Db.UserEventRatings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserEventRating(UserEventRating instance)
        {
            var cache = Db.UserEventRatings.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.EventID = instance.EventID;
				cache.Mark = instance.Mark;
                Db.UserEventRatings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserEventRating(int idUserEventRating)
        {
            UserEventRating instance = Db.UserEventRatings.FirstOrDefault(p => p.ID == idUserEventRating);
            if (instance != null)
            {
                Db.UserEventRatings.DeleteOnSubmit(instance);
                Db.UserEventRatings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}