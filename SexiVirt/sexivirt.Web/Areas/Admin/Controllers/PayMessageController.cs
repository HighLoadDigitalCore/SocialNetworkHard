using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using sexivirt.Model;
using sexivirt.Web.Models.Mappers;
using sexivirt.Web.Models.ViewModels.User;

namespace sexivirt.Web.Areas.Admin.Controllers
{
    public class PayMessageController : AdminController
    {
        //
        // GET: /Admin/PayMessage/
        public ActionResult Index(int? page)
        {
            return View(Repository.MessagePayments.OrderBy(p => p.ID).ToPagedList(page ?? 1, PageSize));
        }

        public ActionResult Create()
        {
            var userView = new MessagePayment();
            return View("Edit", userView);
        }
        public ActionResult Delete(int id)
        {
            Repository.DeleteMessagePayment(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = Repository.MessagePayments.FirstOrDefault(p => p.ID == id);

            if (user != null)
            {
                return View(user);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(MessagePayment user)
        {
            if (user.Duration > 0 && user.Price > 0)
            {
                if (user.ID == 0)
                {
                    Repository.CreatePayment(user);
                }
                else
                {
                    Repository.UpdatePayment(user);
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Необходимо указать цену и длительность");
            }
            return View(user);
        }

	}
}