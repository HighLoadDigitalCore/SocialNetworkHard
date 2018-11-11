using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sexivirt.Model
{
    public partial class SqlRepository
    {
        public IQueryable<Role> Roles
        {
            get
            {
                return Db.Roles;
            }
        }

        public bool CreateRole(Role instance)
        {
            if (instance.ID == 0)
            {
                Db.Roles.InsertOnSubmit(instance);
                Db.Roles.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveRole(int idRole)
        {
            Role instance = Db.Roles.FirstOrDefault(p => p.ID == idRole);
            if (instance != null)
            {
                Db.Roles.DeleteOnSubmit(instance);
                Db.Roles.Context.SubmitChanges();
                return true;
            }

            return false;
        }

    }
}
