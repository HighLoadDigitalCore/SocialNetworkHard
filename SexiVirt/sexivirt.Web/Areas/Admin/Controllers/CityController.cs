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
    public class CityController : AdminController
    {
		public ActionResult Index(int page = 1)
        {
			var list = Repository.Cities;
			return View(list.ToPagedList(page, PageSize));
		}

		public ActionResult Create() 
		{
			var cityView = new CityView();
			return View("Edit", cityView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  city = Repository.Cities.FirstOrDefault(p => p.ID == id); 

			if (city != null) {
				var cityView = (CityView)ModelMapper.Map(city, typeof(City), typeof(CityView));
				return View(cityView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(CityView cityView)
        {
            if (ModelState.IsValid)
            {
                var city = (City)ModelMapper.Map(cityView, typeof(CityView), typeof(City));
                if (city.ID == 0)
                {
                    Repository.CreateCity(city);
                }
                else
                {
                    Repository.UpdateCity(city);
                }
                return RedirectToAction("Index");
            }
            return View(cityView);
        }

        public ActionResult Delete(int id)
        {
            var city = Repository.Cities.FirstOrDefault(p => p.ID == id);
            if (city != null)
            {
                    Repository.RemoveCity(city.ID);
            }
			return RedirectToAction("Index");
        }
	}
}