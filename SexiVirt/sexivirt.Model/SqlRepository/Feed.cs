using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Feed> Feeds
        {
            get
            {
                return Db.Feeds;
            }
        }

        public bool CreateFeed(Feed instance)
        {
            if (instance.ID == 0)
            {
                instance.Text = instance.Text ?? string.Empty;
                instance.AddedDate = DateTime.Now;
                Db.Feeds.InsertOnSubmit(instance);
                Db.Feeds.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveFeed(int idFeed)
        {
            Feed instance = Db.Feeds.FirstOrDefault(p => p.ID == idFeed);
            if (instance != null)
            {
                Db.Feeds.DeleteOnSubmit(instance);
                Db.Feeds.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ReadFeed(Feed feed)
        {
            var instance = Db.Feeds.FirstOrDefault(p => p.ID == feed.ID);
            if (instance != null)
            {
                instance.IsNew = false;
                //Db.Feeds.DeleteOnSubmit(instance);
                Db.Feeds.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}