using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<City> Cities
        {
            get
            {
                return Db.Cities;
            }
        }

        public bool CreateCity(City instance)
        {
            if (instance.ID == 0)
            {
                Db.Cities.InsertOnSubmit(instance);
                Db.Cities.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCity(City instance)
        {
            var cache = Db.Cities.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.Cities.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCity(int idCity)
        {
            City instance = Db.Cities.FirstOrDefault(p => p.ID == idCity);
            if (instance != null)
            {
                Db.Cities.DeleteOnSubmit(instance);
                Db.Cities.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}