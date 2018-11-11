using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Web.Global.Auth;
using sexivirt.Web.Models.ViewModels;
using Tool;
using sexivirt.Web.Mail;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels.User;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class UserController : DefaultController
    {

        public ActionResult Socials()
        {
            var result = SocialAuthResult.CheckAuth();
            if (result.HasResult && result.User != null)
            {
                Auth.Login(result.User.Login); 
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Index(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                if (CurrentUser != null && CurrentUser.ID == user.ID)
                {
                    return RedirectToAction("Home", "Home");
                }
                return View(user);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            if (CurrentUser != null)
            {
                var userView = (UserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(UserView));
                return View(userView);
            }
            return RedirectToLoginPage;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(UserView userView)
        {
            if (CurrentUser.ID == userView.ID)
            {
                if (ModelState.IsValid)
                {
                    var user = (User)ModelMapper.Map(userView, typeof(UserView), typeof(User));
                    Repository.UpdateUser(user);

                    return RedirectToAction("Index");
                }
                return View(userView);
            }
            return RedirectToLoginPage;
        }

        [HttpGet]
        public ActionResult Register()
        {
            var registerUserView = new RegisterUserView()
            {
                Sex = true
            };
            return PartialView(registerUserView);
        }
        [HttpGet]
        public ActionResult Password()
        {
            var registerUserView = new PasswordUserView();
            return PartialView(registerUserView);
        }
        [HttpPost]
        public ActionResult Password(PasswordUserView view)
        {
            var user = Repository.Users.FirstOrDefault(x => x.Email.ToLower() == (view.Email??"").Trim().ToLower());
            if (user == null)
            {
                ViewBag.Message = "Пользователь с таким e-mail не найден в базе данных";
            }
            else
            {
                NotifyMail.SendNotify("ForgotPassword", user.Email,
                            format => string.Format(format, HostName),
                            format => string.Format(format, user.Email, user.Password, HostName));
                ViewBag.Message = "Данные для доступа на сайт отправлены на ваш e-mail";

            }

            return PartialView(view);
        }


        [HttpPost]
        public ActionResult Register(RegisterUserView registerUserView)
        {
            if (registerUserView.Age < 18)
            {
                ModelState.AddModelError("Age", "До 18 лет вход воспрещен");
            }

            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(registerUserView, typeof(RegisterUserView), typeof(User));
                user.AvatarPath = "";
                user.Login = user.Email;
                Repository.CreateUser(user);

                NotifyMail.SendNotify("Register", user.Email,
                                      format => string.Format(format, HostName),
                                      format => string.Format(format, user.ActivatedLink, HostName));
                //Auth.Login(user.Login);
                ViewBag.Message =
                    "Вы успешно зарегистрировались на сайте. На ваш почтовый ящик выслана ссылка для активации. Чтобы зайти на сайт, пожалуйста, перейдите по указанной ссылке.";
                return PartialView(registerUserView);
                //return PartialView("_Ok");
            }
            return PartialView(registerUserView);
        }


        public ActionResult Rating(int id, bool mark)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            if (CurrentUser != null && user != null)
            {
                var entry = Repository.UserRatings.FirstOrDefault(p => p.ReceiverID == id && p.SenderID == CurrentUser.ID);
                if (entry != null)
                {
                    if ((entry.Mark == 1 && mark) || (entry.Mark == -1 && !mark))
                    {
                        return PartialView(user);
                    }
                    else
                    {
                        Repository.RemoveUserRating(entry.ID);
                    }
                }
                else
                {
                    Repository.CreateUserRating(new UserRating
                    {
                        ReceiverID = user.ID,
                        SenderID = CurrentUser.ID,
                        Mark = mark ? 1 : -1
                    });
                }
            }
            var resultUser = Repository.Users.FirstOrDefault(p => p.ID == id);
            return PartialView(resultUser);
        }

        /*
        public ActionResult Activate(string id)
        {
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.ActivatedLink, id, true) == 0);
            if (user != null)
            {
                Repository.ActivateUser(user);
                Auth.Login(user.Login);
                return View("ActivateSuccess");
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            var changePasswordView = new ChangePasswordView
            {
                ID = CurrentUser.ID
            };
            return View(changePasswordView);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordView changePasswordView)
        {
            if (ModelState.IsValid)
            {
                CurrentUser.Password = changePasswordView.NewPassword;
                Repository.ChangePassword(CurrentUser);
                TempData["message"] = "Saved";
            }
            return View(changePasswordView);
        }*/
    }
}