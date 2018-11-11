using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels.User;
using PagedList;

namespace sexivirt.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : AdminController
    {
        public ActionResult Index(int? page, string word)
        {
            var list = Repository.Users;
            if (!string.IsNullOrEmpty(word))
            {
                list = list.Where(x => SqlMethods.Like(x.FirstName.ToLower(), string.Format("%{0}%", word.ToLower())) || SqlMethods.Like(x.Email.ToLower(), string.Format("%{0}%", word.ToLower())));
            }
            ViewBag.Page = page ?? 1;
            if (list.Count() < ((page ?? 1)-1)*PageSize)
            {
                ViewBag.Page = 1;
                page = 1;
            }
            ViewBag.Word = word;
            return View(list.OrderBy(p => p.ID).ToPagedList(page ?? 1, PageSize));

        }

        public ActionResult Create()
        {
            var userView = new UserView();
            return View("Edit", userView);
        }

        [HttpGet]
        public ActionResult Edit(int id, int? page, string word)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.Word = word;

            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            if (user != null)
            {
                var userView = (UserView)ModelMapper.Map(user, typeof(User), typeof(UserView));
                return View(userView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(UserView userView, int? page, string word)
        {
            ViewBag.Page = page ?? 1;
            ViewBag.Word = word;

            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(userView, typeof(UserView), typeof(User));
                if (user.ID == 0)
                {
                    Repository.CreateUser(user);
                }
                else
                {
                    Repository.UpdateUserAdmin(user);
                }
                return RedirectToAction("Index", new { page = page, word = word });
            }
            return View(userView);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Login(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Auth.Login(user.Login);
            }
            return RedirectToAction("Index", "Home", new { area = "Default" });
        }
    }
}