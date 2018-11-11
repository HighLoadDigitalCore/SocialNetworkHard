using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using sexivirt.Model;
using sexivirt.Web.Areas.Default.Controllers;

namespace sexivirt.Web.Areas.Admin.Controllers
{
    public class MoneyController : AdminController
    {
        //
        // GET: /Admin/Money/
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            var list = Repository.MoneyWithdraws.OrderByDescending(x => x.AddedDate).ToList();
            foreach (var withdraw in list)
            {
                Repository.RecalculateUserMoney(withdraw.UserID);
            }
            return View(list.ToPagedList(page, PageSize));
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var w = Repository.MoneyWithdraws.FirstOrDefault(x => x.ID == id);
            if (w == null)
                return RedirectToAction("Index");
            return View(w);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Edit(MoneyWithdraw w)
        {
            Repository.UpdateMoneyWithdrawStatus(w);

            return RedirectToAction("Index");

        }

	}
}