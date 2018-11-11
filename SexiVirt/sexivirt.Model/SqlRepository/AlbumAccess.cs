using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<AlbumAccess> AlbumAccesses
        {
            get
            {
                return Db.AlbumAccesses;
            }
        }

        public bool CreateAlbumAccess(AlbumAccess instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.AlbumAccesses.InsertOnSubmit(instance);
                Db.AlbumAccesses.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateAlbumAccess(AlbumAccess instance)
        {
            var cache = Db.AlbumAccesses.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.AlbumID = instance.AlbumID;
				cache.AddedDate = instance.AddedDate;
                Db.AlbumAccesses.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveAlbumAccess(int idAlbumAccess)
        {
            AlbumAccess instance = Db.AlbumAccesses.FirstOrDefault(p => p.ID == idAlbumAccess);
            if (instance != null)
            {
                var feeds = instance.Feeds.ToList();
                Db.Feeds.DeleteAllOnSubmit(feeds);
                
                Db.AlbumAccesses.DeleteOnSubmit(instance);
                Db.AlbumAccesses.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}