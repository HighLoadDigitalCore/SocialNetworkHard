using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using sexivirt.Model.Proxy;
using Tool;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class SettingsController : DefaultController
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {

            var settings = new UserSettings() {MyMail = CurrentUser.Email};
            return View(settings);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(UserSettings settings, FormCollection collection)
        {
            if (collection.AllKeys.Contains("pass"))
            {
                var error = "";
                if (settings.MyPassword.IsNullOrWhiteSpace())
                {
                    error = "Необходимо указать ваш текущий пароль";
                }
                else if (settings.MyPassword.Trim() != CurrentUser.Password)
                {
                    error = "Ваш текущий пароль указан неверно";
                }
                else if (settings.NewPassword.IsNullOrWhiteSpace() || settings.NewPasswordRepeat.IsNullOrWhiteSpace())
                {
                    error = "Новый пароль не указан";
                }
                else if (settings.NewPassword.Trim().Length<4)
                {
                    error = "Длина пароля должна быть не менее 4 символов";
                }
                else if (settings.NewPassword.Trim() != settings.NewPasswordRepeat.Trim())
                {
                    error = "Пароли не совпадают";
                }
                if (!error.IsNullOrWhiteSpace())
                {
                    ViewBag.ErrorPass = error;
                }
                else
                {
                    CurrentUser.Password = settings.NewPassword;
                    Repository.ChangePassword(CurrentUser);
                    ViewBag.ErrorPass = "Ваш пароль успешно изменен";
                }
            }
            if (collection.AllKeys.Contains("mail"))
            {
                var error = "";
                if (settings.NewMail.IsNullOrWhiteSpace() || !settings.NewMail.IsEmail())
                {
                    error = "Необходимо указать корректный e-mail";
                }
                else if (settings.MyPassForMail.IsNullOrWhiteSpace())
                {
                    error = "Необходимо указать ваш текущий пароль";
                }
                else if (settings.MyPassForMail.Trim()!= CurrentUser.Password)
                {
                    error = "Ваш текщий пароль указан неверно";
                }
                if (!error.IsNullOrWhiteSpace())
                {
                    ViewBag.ErrorMail = error;
                }
                else
                {
                    CurrentUser.Email = settings.NewMail;
                    Repository.ChangeMail(CurrentUser);
                    ViewBag.ErrorMail = "Ваш e-mail успешно изменен";
                }
            }
            settings.MyMail = CurrentUser.Email;
            return View(settings);
        }
    }
}
