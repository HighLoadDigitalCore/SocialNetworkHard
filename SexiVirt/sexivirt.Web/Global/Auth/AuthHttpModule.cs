using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Global.Auth
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.Authenticate);
        }

        private void Authenticate(Object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.AuthCookieProvider = new HttpContextCookieProvider(app.Context);
            app.Context.User = auth.CurrentUser;
        }

        public void Dispose()
        {
        }
    }
}