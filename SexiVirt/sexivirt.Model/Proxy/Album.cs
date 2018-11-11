using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Album
    {
        public int PhotosCount
        {
            get
            {
                return Photos.Count();
            }
        }

        public string FirstPhoto
        {
            get
            {
                if (Photos.Any()) 
                {
                    return Photos.OrderBy(p => p.IsCover ? 0 : 1).First().FilePath;
                }
                return string.Empty;
            }
        }

        public string SecondPhoto
        {
            get
            {
                if (Photos.Skip(1).Any())
                {
                    return Photos.Skip(1).First().FilePath;
                }
                return string.Empty;
            }
        }

        public bool HasAccess(User user)
        {
            if (Price.HasValue && Price > 0)
            {
                if (user != null)
                {
                    return AlbumAccesses.Any(p => p.UserID == user.ID) || UserID == user.ID;
                }
                return false;
            }
            return true;
        }

        public IEnumerable<Photo> SubPhotos
        {
            get
            {
                return Photos.AsEnumerable();
            }
        }
	}
}