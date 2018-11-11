using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserEvent> UserEvents
        {
            get
            {
                return Db.UserEvents;
            }
        }

        public bool CreateUserEvent(UserEvent instance)
        {
            if (instance.ID == 0)
            {
                Db.UserEvents.InsertOnSubmit(instance);
                Db.UserEvents.Context.SubmitChanges();
                return true;
            }

            return false;
        }

      
        public bool RemoveUserEvent(int idUserEvent)
        {
            UserEvent instance = Db.UserEvents.FirstOrDefault(p => p.ID == idUserEvent);
            if (instance != null)
            {
                Db.UserEvents.DeleteOnSubmit(instance);
                Db.UserEvents.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}