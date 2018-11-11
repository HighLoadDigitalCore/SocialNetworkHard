using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using sexivirt.Web.Controllers;

namespace sexivirt.Web.Areas.Admin.Controllers
{
    public abstract class AdminController : BaseController
    {
        public readonly int PageSize = 20;
        protected override void Initialize(RequestContext requestContext)
        {
            CultureInfo ci = new CultureInfo("ru");

            Thread.CurrentThread.CurrentCulture = ci;
            base.Initialize(requestContext);
        }

    }
}