using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ChangePassword> ChangePasswords
        {
            get
            {
                return Db.ChangePasswords;
            }
        }

        public bool CreateChangePassword(ChangePassword instance)
        {
            if (instance.ID == 0)
            {
                Db.ChangePasswords.InsertOnSubmit(instance);
                Db.ChangePasswords.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateChangePassword(ChangePassword instance)
        {
            var cache = Db.ChangePasswords.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.Code = instance.Code;
				cache.AddedDate = instance.AddedDate;
				cache.Email = instance.Email;
				cache.IsUsed = instance.IsUsed;
                Db.ChangePasswords.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveChangePassword(int idChangePassword)
        {
            ChangePassword instance = Db.ChangePasswords.FirstOrDefault(p => p.ID == idChangePassword);
            if (instance != null)
            {
                Db.ChangePasswords.DeleteOnSubmit(instance);
                Db.ChangePasswords.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}