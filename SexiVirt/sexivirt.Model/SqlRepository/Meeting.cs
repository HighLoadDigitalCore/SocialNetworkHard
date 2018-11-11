using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Meeting> Meetings
        {
            get
            {
                return Db.Meetings;
            }
        }

        public bool CreateMeeting(Meeting instance)
        {
            if (instance.ID == 0)
            {
                Db.Meetings.InsertOnSubmit(instance);
                Db.Meetings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMeeting(Meeting instance)
        {
            var cache = Db.Meetings.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.CityID = instance.CityID;
				cache.MeetingDate = instance.MeetingDate;
				cache.Text = instance.Text;
                Db.Meetings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMeeting(int idMeeting)
        {
            Meeting instance = Db.Meetings.FirstOrDefault(p => p.ID == idMeeting);
            if (instance != null)
            {
                Db.Meetings.DeleteOnSubmit(instance);
                Db.Meetings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}