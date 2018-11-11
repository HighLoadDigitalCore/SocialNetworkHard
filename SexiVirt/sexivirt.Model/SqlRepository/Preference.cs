using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Preference> Preferences
        {
            get
            {
                return Db.Preferences;
            }
        }

        public bool CreatePreference(Preference instance)
        {
            if (instance.ID == 0)
            {
                Db.Preferences.InsertOnSubmit(instance);
                Db.Preferences.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePreference(Preference instance)
        {
            var cache = Db.Preferences.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.Preferences.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePreference(int idPreference)
        {
            Preference instance = Db.Preferences.FirstOrDefault(p => p.ID == idPreference);
            if (instance != null)
            {
                Db.Preferences.DeleteOnSubmit(instance);
                Db.Preferences.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}