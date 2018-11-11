using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserRating> UserRatings
        {
            get
            {
                return Db.UserRatings;
            }
        }

        public bool CreateUserRating(UserRating instance)
        {
            if (instance.ID == 0)
            {
                Db.UserRatings.InsertOnSubmit(instance);
                Db.UserRatings.Context.SubmitChanges();
                UpdateRating(instance.ReceiverID);
                return true;
            }

            return false;
        }

        public bool RemoveUserRating(int idUserRating)
        {
            var instance = Db.UserRatings.FirstOrDefault(p => p.ID == idUserRating);
            if (instance != null)
            {
                var receiverID = instance.ReceiverID;
                Db.UserRatings.DeleteOnSubmit(instance);
                Db.UserRatings.Context.SubmitChanges();
                UpdateRating(receiverID);
                return true;
            }
            return false;
        }

        private void UpdateRating(int userID)
        {
            var user = Db.Users.FirstOrDefault(p => p.ID == userID);
            if (user != null)
            {
                var any = Db.UserRatings.Any(p => p.ReceiverID == user.ID);
                user.Rating = any ? Db.UserRatings.Where(p => p.ReceiverID == user.ID).Sum(p => p.Mark) : 0;
                Db.UserRatings.Context.SubmitChanges();
            }
        } 
    }
}