using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserStatus> UserStatus
        {
            get
            {
                return Db.UserStatus;
            }
        }

        public bool CreateUserStatus(UserStatus instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.UserStatus.InsertOnSubmit(instance);
                Db.UserStatus.Context.SubmitChanges();

                var user = Db.Users.FirstOrDefault(p => p.ID == instance.UserID);
                user.Status = instance.Text;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserStatus(int idUserStatus)
        {
            UserStatus instance = Db.UserStatus.FirstOrDefault(p => p.ID == idUserStatus);
            if (instance != null)
            {
                Db.UserStatus.DeleteOnSubmit(instance);
                Db.UserStatus.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}