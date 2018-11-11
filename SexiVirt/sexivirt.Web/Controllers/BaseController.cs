using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using sexivirt.Web.Global.Auth;
using sexivirt.Web.Global.Config;
using sexivirt.Model;
using sexivirt.Web.Models.Mappers;
using System.Net;

namespace sexivirt.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public static string HostName = string.Empty;

        protected static string NotFoundPage = "~/not-found-page";

        protected static string LoginPage = "~/Login";

        [Inject]
        public IRepository Repository { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        [Inject]
        public IConfig Config { get; set; }

        public ModelMapper ModelMapper = new ModelMapper();

        public User CurrentUser
        {
            get
            {
                if (Auth.CurrentUser.Identity is IUserable)
                {
                    return ((IUserable)Auth.CurrentUser.Identity).User;
                }
                return null;
            }
        }

        public RedirectResult RedirectToNotFoundPage
        {
            get
            {
                return Redirect(NotFoundPage);
            }
        }


        public RedirectResult RedirectToLoginPage
        {
            get
            {
                return Redirect(LoginPage);
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url != null)
            {
                HostName = requestContext.HttpContext.Request.Url.Authority;
            }
            base.Initialize(requestContext);
        }

        protected void Save(string url, string path)
        {

            var webClient = new WebClient();
            var bytes = webClient.DownloadData(url);

            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            
        }

        public void UpdateOnline()
        {

            var admin = Repository.Users.FirstOrDefault(x => x.Login == "admin");
            if (admin != null)

            {
                var rnd = new Random(DateTime.Now.Millisecond);
                var lastTime = ((DateTime?) Session["LastTime"]) ?? DateTime.MinValue;
                if (lastTime < DateTime.Now.AddMinutes(-5))
                {
                    Session["LastTime"] = DateTime.Now;
                    var list = Repository.ChatUsers.Select(x => x.User).ToList();

                    foreach (var user in list)
                    {
/*
                        if (admin.IsOnline)
                        {
                            user.LastVisitDate = DateTime.Now;//.AddMinutes(4.9);
                            Repository.UpdateUserVisitDate(user);
                        }
                        else
*/
                        {
                            bool d = false;
                            if ( (DateTime.Now.Hour > 12 && DateTime.Now.Hour < 24) || (DateTime.Now.Hour>0 && DateTime.Now.Hour<1))
                            {
                                d = rnd.Next(100, 999) > 300;
                            }
                            else
                            {
                                d = rnd.Next(100, 999) > 700;
                            }
                            if (d)
                            {
                                user.LastVisitDate = DateTime.Now; //.AddMinutes(4.9);
                                Repository.UpdateUserVisitDate(user);
                            }
                            else
                            {
                                user.LastVisitDate = DateTime.Now.AddMinutes(-5); //.AddMinutes(4.9);
                                Repository.UpdateUserVisitDate(user);
                            }
                        }
                    }
                }
            }
        }
    }
}