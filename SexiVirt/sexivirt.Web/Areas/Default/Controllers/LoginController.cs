using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using sexivirt.Web.Mail;
using sexivirt.Web.Models.ViewModels;
using sexivirt.Web.Models.ViewModels.User;
using OhAuthToo.ConcreteAuthorizers;
using System.Configuration;
using OhAuthToo.ConcreteClients;
using Tool;
using sexivirt.Model;
using System.IO;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class LoginController : DefaultController
    {
      
        [HttpGet]
        public ActionResult Ajax(LoginView view, string activate = "")
        {

            if (!activate.IsNullOrWhiteSpace())
            {
                var act = Repository.ActivateUser(activate);
                if (act == null)
                {
                    ViewBag.Message = "Неверная ссылка активации";
                }
                else
                {
                    ViewBag.Message = "Ваша учетная запись активирована";
                    var u = Auth.Login(act.Email, act.Password, true);
                    if (u.FirstName == "Locked")
                    {
                        ViewBag.Message = "Ваша учетная запись заблокирована";
                    }
                    return PartialView("_Ok");
                }

            }

            return PartialView(view ?? new LoginView());
        }

     

        [HttpPost]
        public ActionResult Ajax(LoginView loginView, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    if (user.FirstName == "Locked")
                    {
                        ViewBag.Message = "Ваша учетная запись заблокирована";
                        return PartialView(loginView);
                    }
                    if (!user.ActivatedDate.HasValue)
                    {
                        ViewBag.Message =
                            "Ваша учетная запись не активирована. Чтобы зайти на сайт, необходимо активировать учетную запись по ссылке из письма";
                        Auth.LogOut();
                        return Ajax(loginView);
                    }
                   
                    return PartialView("_Ok");
                }
                ViewBag.Message = "Логин или пароль указаны неверно";
            }
            else
            {
                ViewBag.Message = "Логин или пароль указаны неверно";
            }
            return PartialView(loginView);
        }

        [HttpGet]
        public ActionResult AjaxAside()
        {
            return PartialView(new LoginView());
        }

        [HttpPost]
        public ActionResult AjaxAside(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    if (user.FirstName == "Locked")
                    {
                        ViewBag.Message = "Ваша учетная запись заблокирована";
                        return PartialView(loginView);
                    }
                    return PartialView("_Ok");
                }
                ViewBag.Message = "Логин или пароль указаны неверно";
            }
            else
            {
                /*ViewBag.Message = "Логин или пароль указаны неверно";*/
            }
            return PartialView(loginView);
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult Index(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Password doesn't match");
            }
            return View(loginView);
        }

        public RedirectResult VKLogOn()
        {
            var vkAuth = new VkontakteAuthorizer();
            vkAuth.ClientId = ConfigurationManager.AppSettings["VkClientId"];
            //vkAuth.Scope = "user_work_history,friends_work_history,publish_stream,read_friendlists,contacts";
            vkAuth.Scope = "";
            vkAuth.RedirectUri = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + ":" + Request.Url.Port + "/Login/VKReturn";
            return Redirect(vkAuth.CodeRequestUri);
        }

        public ActionResult VKReturn(string code)
        {
            var vkAuth = new VkontakteAuthorizer();
            vkAuth.ClientId = ConfigurationManager.AppSettings["VkClientId"];
            vkAuth.ClientSecret = ConfigurationManager.AppSettings["VkClientSecret"];
            vkAuth.RedirectUri = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + ":" + Request.Url.Port + "/Login/VKReturn";
            var response = vkAuth.GetAuthorizationResponse(code);

            var client = new VkontakteClient();
            client.Response = response;

            string tmp = client.Permissions();

            var user = Repository.Users.SingleOrDefault(u => u.Login == "vk_" + client.UserId);

            // need registration
            if (user == null)
            {
                var model = new VKSignUpViewModel
                {
                    UserId =
                        client.UserId,
                    Name = client.FirstName,
                    Surname = client.LastName,
                    Birthday = client.Birthdate,
                    Sex = client.Gender == OhAuthToo.Utils.Gender.Male
                };

                if (!string.IsNullOrWhiteSpace(client.PhotoUrl))
                {
                    var destinationDir = "/Content/files/avatars/";
                    var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(client.PhotoUrl);
                    var filePath = Server.MapPath(destinationDir + uFile);
                    Save(client.PhotoUrl, filePath);
                    model.AvatarPath = destinationDir + uFile;
                }
                else
                {
                    model.AvatarPath = "";
                }
                var vkUser = (User)ModelMapper.Map(model, typeof(VKSignUpViewModel), typeof(User));
                vkUser.Password = StringExtension.CreateRandomPassword(10);
                vkUser.Login = "vk_" + model.UserId;
                vkUser.ActivatedLink = "";
                Repository.CreateUser(vkUser);
                Auth.Login(vkUser.Login);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Auth.Login(user.Login);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult VKSingUp(VKSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {


            }
            return View(model);
        }


        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordView());
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordView forgotPasswordView)
        {
            if (ModelState.IsValid)
            {
                var user =
                    Repository.Users.FirstOrDefault(p => string.Compare(p.Email, forgotPasswordView.Email, true) == 0);
                if (user != null)
                {
                    NotifyMail.SendNotify("ForgotPassword", user.Email,
                                                format => string.Format(format, HostName),
                                                format => string.Format(format, user.Email, user.Password, HostName));
                    return View("ForgotPasswordSuccess");
                }
                ModelState.AddModelError("Email", "Email user not found");
            }
            return View(forgotPasswordView);
        }
    }
}