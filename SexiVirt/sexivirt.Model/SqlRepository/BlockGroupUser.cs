using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BlockGroupUser> BlockGroupUsers
        {
            get
            {
                return Db.BlockGroupUsers;
            }
        }

        public bool CreateBlockGroupUser(BlockGroupUser instance)
        {
            if (instance.ID == 0)
            {
                Db.BlockGroupUsers.InsertOnSubmit(instance);
                Db.BlockGroupUsers.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBlockGroupUser(BlockGroupUser instance)
        {
            var cache = Db.BlockGroupUsers.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.GroupID = instance.GroupID;
                Db.BlockGroupUsers.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBlockGroupUser(int idBlockGroupUser)
        {
            BlockGroupUser instance = Db.BlockGroupUsers.FirstOrDefault(p => p.ID == idBlockGroupUser);
            if (instance != null)
            {
                Db.BlockGroupUsers.DeleteOnSubmit(instance);
                Db.BlockGroupUsers.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}