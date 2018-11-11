using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Photo> Photos
        {
            get
            {
                return Db.Photos;
            }
        }

        public bool CreatePhoto(Photo instance)
        {
            if (instance.ID == 0)
            {
                Db.Photos.InsertOnSubmit(instance);
                Db.Photos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePhoto(Photo instance)
        {
            var cache = Db.Photos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.AlbumID = instance.AlbumID;
				cache.FilePath = instance.FilePath;
				cache.IsCover = instance.IsCover;
                Db.Photos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePhoto(int idPhoto)
        {
            Photo instance = Db.Photos.FirstOrDefault(p => p.ID == idPhoto);
            if (instance != null)
            {
                Db.Photos.DeleteOnSubmit(instance);
                Db.Photos.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool UpdateAlbumPhotos(List<Photo> photos, int idAlbum)
        {
            var album = Db.Albums.FirstOrDefault(p => p.ID == idAlbum);
            if (album != null)
            {
                var list = album.Photos.ToList();
                var filePaths = photos.Select(p => p.FilePath);
                var listForDelete = list.Where(p => !filePaths.Contains(p.FilePath)).ToList(); 
                var existList = list.Where(p => !filePaths.Contains(p.FilePath));
                var newList = photos.Where(p => p.ID == 0);
                Db.Photos.DeleteAllOnSubmit(listForDelete);
                Db.Photos.Context.SubmitChanges();

                //удалить кавер
                var cover =  Db.Photos.FirstOrDefault(p => p.AlbumID == idAlbum && p.IsCover);
                if (cover != null) 
                {
                    cover.IsCover = false;
                    Db.Photos.Context.SubmitChanges();
                }

                foreach (var item in newList)
                {
                    var photo = new Photo()
                    {
                        FilePath = item.FilePath,
                        AlbumID = album.ID,
                    };
                    Db.Photos.InsertOnSubmit(photo);
                    Db.Photos.Context.SubmitChanges();
                }
                //Установить кавер
                if (photos.Any(p => p.IsCover))
                {
                    var coverPhoto = photos.FirstOrDefault(p => p.IsCover);
                    var existPhoto = Db.Photos.FirstOrDefault(p => p.FilePath == coverPhoto.FilePath);
                    if (existPhoto != null)
                    {
                        existPhoto.IsCover = true;
                        Db.Photos.Context.SubmitChanges();
                    }
                }
                return true;
            }
            return false;
        }

        public Photo ChangePhoto(int idCurrentPhoto, bool next)
        {
            var currentPhoto = Db.Photos.FirstOrDefault(p => p.ID == idCurrentPhoto);

            if (currentPhoto != null)
            {
                var albumID = currentPhoto.AlbumID;

                Photo photo = null;
                if (next)
                {
                    //next 
                    photo = Db.Photos.Where(p => p.AlbumID == albumID && p.ID > currentPhoto.ID).OrderBy(p => p.ID).FirstOrDefault();
                    //if last - get first
                    if (photo == null)
                    {
                        photo = Db.Photos.Where(p => p.AlbumID == albumID).OrderBy(p => p.ID).FirstOrDefault();
                    }
                }
                else
                {
                    //next 
                    photo = Db.Photos.Where(p => p.AlbumID == albumID && p.ID < currentPhoto.ID).OrderByDescending(p => p.ID).FirstOrDefault();
                    //if first - get last
                    if (photo == null)
                    {
                        photo = Db.Photos.Where(p => p.AlbumID == albumID).OrderByDescending(p => p.ID).FirstOrDefault();
                    }
                }
                return photo;
            }
            return null;
        }

        public Photo ChangeAllPhoto(int idCurrentPhoto, bool next)
        {
            var currentPhoto = Db.Photos.FirstOrDefault(p => p.ID == idCurrentPhoto);

            if (currentPhoto != null)
            {
                var userID = currentPhoto.Album.UserID;

                Photo photo = null;
                if (next)
                {
                    //next 
                    photo = Db.Photos.Where(p => p.Album.UserID == userID && p.ID > currentPhoto.ID).OrderBy(p => p.ID).FirstOrDefault();
                    //if last - get first
                    if (photo == null)
                    {
                        photo = Db.Photos.Where(p => p.Album.UserID == userID).OrderBy(p => p.ID).FirstOrDefault();
                    }
                }
                else
                {
                    //next 
                    photo = Db.Photos.Where(p => p.Album.UserID == userID && p.ID < currentPhoto.ID).OrderByDescending(p => p.ID).FirstOrDefault();
                    //if first - get last
                    if (photo == null)
                    {
                        photo = Db.Photos.Where(p => p.Album.UserID == userID).OrderByDescending(p => p.ID).FirstOrDefault();
                    }
                }
                return photo;
            }
            return null;
        }
    }
}