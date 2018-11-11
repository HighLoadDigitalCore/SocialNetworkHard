using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Photo
    {
        public int AlbumCount
        {
            get
            {
               return Album.PhotosCount;
            }
        }

        public int NumberOfAlbum
        {
            get
            {
                return Album.Photos.Count(p => p.ID <= ID);
            }
        }

        public int UserCount
        {
            get
            {
                return Album.User.PhotosCount;
            }
        }

        public int NumberOfUser
        {
            get
            {
                return Album.User.Photos.Count(p => p.ID <= ID);
            }
        }

    }
}