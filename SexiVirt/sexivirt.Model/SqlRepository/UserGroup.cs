using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserGroup> UserGroups
        {
            get
            {
                return Db.UserGroups;
            }
        }

        public bool CreateUserGroup(UserGroup instance)
        {
            if (instance.ID == 0)
            {
                Db.UserGroups.InsertOnSubmit(instance);
                Db.UserGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserGroup(UserGroup instance)
        {
            var cache = Db.UserGroups.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.GroupID = instance.GroupID;
                Db.UserGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserGroup(int idUserGroup)
        {
            UserGroup instance = Db.UserGroups.FirstOrDefault(p => p.ID == idUserGroup);
            if (instance != null)
            {
                Db.UserGroups.DeleteOnSubmit(instance);
                Db.UserGroups.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}