using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Album> Albums
        {
            get
            {
                return Db.Albums;
            }
        }

        public bool CreateAlbum(Album instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Albums.InsertOnSubmit(instance);
                Db.Albums.Context.SubmitChanges();
                return true;
            }

            return false;
        }

     public bool DeleteAlbum(int id)
     {
         var album = Db.Albums.FirstOrDefault(x => x.ID == id);
         if (album != null)
         {
             var photos = Db.Photos.Where(x => x.AlbumID == album.ID);
             if (photos.Any())
             {
                 Db.Photos.DeleteAllOnSubmit(photos);
                 Db.Photos.Context.SubmitChanges();
             }
             Db.Albums.DeleteOnSubmit(album);
             Db.Albums.Context.SubmitChanges();
         }
         return true;
     }

     public bool UpdateAlbum(Album instance)
        {
            var cache = Db.Albums.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Price = instance.Price;
                Db.Albums.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveAlbum(int idAlbum)
        {
            Album instance = Db.Albums.FirstOrDefault(p => p.ID == idAlbum);
            if (instance != null)
            {
                Db.Albums.DeleteOnSubmit(instance);
                Db.Albums.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}