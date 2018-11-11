using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Web.Models.ViewModels;
using sexivirt.Model;
using PagedList;


namespace sexivirt.Web.Areas.Admin.Controllers
{ 
    public class PreferenceController : AdminController
    {
		public ActionResult Index(int page = 1)
        {
			var list = Repository.Preferences;
			return View(list.ToPagedList(page, PageSize));
		}

		public ActionResult Create() 
		{
			var preferenceView = new PreferenceView();
			return View("Edit", preferenceView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  preference = Repository.Preferences.FirstOrDefault(p => p.ID == id); 

			if (preference != null) {
				var preferenceView = (PreferenceView)ModelMapper.Map(preference, typeof(Preference), typeof(PreferenceView));
				return View(preferenceView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(PreferenceView preferenceView)
        {
            if (ModelState.IsValid)
            {
                var preference = (Preference)ModelMapper.Map(preferenceView, typeof(PreferenceView), typeof(Preference));
                if (preference.ID == 0)
                {
                    Repository.CreatePreference(preference);
                }
                else
                {
                    Repository.UpdatePreference(preference);
                }
                return RedirectToAction("Index");
            }
            return View(preferenceView);
        }

        public ActionResult Delete(int id)
        {
            var preference = Repository.Preferences.FirstOrDefault(p => p.ID == id);
            if (preference != null)
            {
                    Repository.RemovePreference(preference.ID);
            }
			return RedirectToAction("Index");
        }
	}
}