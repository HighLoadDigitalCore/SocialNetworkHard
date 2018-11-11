using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class SettingsController : AdminController
    {
     
        [HttpGet]
        public ActionResult Index()
        {
            return View(Repository.Settings.ToList());
        }   
        
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            foreach (var key in collection.AllKeys)
            {
                var setting = Repository.GetSetting(key);
                if (setting != null)
                {
                    Type type = setting.Type == "CheckBox" ? typeof (bool) : typeof (string);
                    Repository.UpdateSetting(key,
                        collection.GetValue(key).ConvertTo(type).ToString());
                }
            }
            return View(Repository.Settings.ToList());
        }

	}
}