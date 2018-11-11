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
    public class GiftController : AdminController
    {
		public ActionResult Index(int page = 1)
        {
			var list = Repository.Gifts;
            return View(list.ToPagedList(page, PageSize));
		}

		public ActionResult Create() 
		{
            var giftView = new GiftView()
            {
                IsActive = true
            };
			return View("Edit", giftView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  gift = Repository.Gifts.FirstOrDefault(p => p.ID == id); 

			if (gift != null) 
            {
				var giftView = (GiftView)ModelMapper.Map(gift, typeof(Gift), typeof(GiftView));
				return View(giftView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(GiftView giftView)
        {
            if (ModelState.IsValid)
            {
                var gift = (Gift)ModelMapper.Map(giftView, typeof(GiftView), typeof(Gift));
                if (gift.ID == 0)
                {
                    Repository.CreateGift(gift);
                }
                else
                {
                    Repository.UpdateGift(gift);
                }
                return RedirectToAction("Index");
            }
            return View(giftView);
        }

        public ActionResult Delete(int id)
        {
            var gift = Repository.Gifts.FirstOrDefault(p => p.ID == id);
            if (gift != null)
            {
                    Repository.RemoveGift(gift.ID);
            }
			return RedirectToAction("Index");
        }
	}
}