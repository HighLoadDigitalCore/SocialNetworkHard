using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Connect> Connects
        {
            get
            {
                return Db.Connects;
            }
        }

        public bool CreateConnect(Connect instance)
        {
            if (instance.ID == 0)
            {
                Db.Connects.InsertOnSubmit(instance);
                Db.Connects.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateConnect(Connect instance)
        {
            var cache = Db.Connects.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.OtherUserID = instance.OtherUserID;
                Db.Connects.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveConnect(int idConnect)
        {
            Connect instance = Db.Connects.FirstOrDefault(p => p.ID == idConnect);
            if (instance != null)
            {
                Db.Connects.DeleteOnSubmit(instance);
                Db.Connects.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}