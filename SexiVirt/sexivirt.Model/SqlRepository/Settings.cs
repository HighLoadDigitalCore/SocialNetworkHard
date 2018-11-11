using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sexivirt.Model
{
    public partial class Setting
    {
       
        public static string Get(string name)
        {
            return
                (new sexivirtDbDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
                    .Settings.FirstOrDefault(x => x.Name == name) ?? new Setting()).Value;
        }
    }

    public partial class SqlRepository
    {
        public IQueryable<Setting> Settings
        {
            get { return Db.Settings; }
        }

        public Setting GetSetting(string name)
        {
            return Db.Settings.FirstOrDefault(x => x.Name == name);
        }

        public bool UpdateSetting(string name, string value)
        {
            var setting = Db.Settings.FirstOrDefault(x => x.Name == name);
            if (setting != null)
            {
                setting.Value = value;
                Db.Settings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}
