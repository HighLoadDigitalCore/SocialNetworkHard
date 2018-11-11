using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Event> Events
        {
            get
            {
                return Db.Events;
            }
        }

        public bool CreateEvent(Event instance)
        {
            if (instance.ID == 0)
            {
                instance.Rating = 0;
                Db.Events.InsertOnSubmit(instance);
                Db.Events.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateEvent(Event instance)
        {
            var cache = Db.Events.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Description = instance.Description;
				cache.ImagePath = instance.ImagePath;
				cache.EventDate = instance.EventDate;
				cache.CityID = instance.CityID;
				cache.Place = instance.Place;
				cache.Coordinate = instance.Coordinate;
                Db.Events.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveEvent(int idEvent)
        {
            Event instance = Db.Events.FirstOrDefault(p => p.ID == idEvent);
            if (instance != null)
            {
                var feeds = instance.Feeds.ToList();
                Db.Feeds.DeleteAllOnSubmit(feeds);
                
                Db.Events.DeleteOnSubmit(instance);
                Db.Events.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}