using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using sexivirt.Web.Global.Auth;

namespace sexivirt.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            DefaultModelBinder.ResourceClassKey = "Messages";

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (this.Context.Session != null)
            {
                var auth = DependencyResolver.Current.GetService<IAuthentication>();
                auth.AuthCookieProvider = new HttpContextCookieProvider(this.Context);
                this.Context.User = auth.CurrentUser;
            }
        }
    }
}
