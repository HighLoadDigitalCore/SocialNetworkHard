using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFoundPage()
        {
            return View();
        }

    }
}