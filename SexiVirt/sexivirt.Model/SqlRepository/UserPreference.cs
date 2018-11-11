using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserPreference> UserPreferences
        {
            get
            {
                return Db.UserPreferences;
            }
        }

        public bool CreateUserPreference(UserPreference instance)
        {
            if (instance.ID == 0)
            {
                Db.UserPreferences.InsertOnSubmit(instance);
                Db.UserPreferences.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserPreference(UserPreference instance)
        {
            var cache = Db.UserPreferences.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.PreferenceID = instance.PreferenceID;
                Db.UserPreferences.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserPreference(int idUserPreference)
        {
            UserPreference instance = Db.UserPreferences.FirstOrDefault(p => p.ID == idUserPreference);
            if (instance != null)
            {
                Db.UserPreferences.DeleteOnSubmit(instance);
                Db.UserPreferences.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}