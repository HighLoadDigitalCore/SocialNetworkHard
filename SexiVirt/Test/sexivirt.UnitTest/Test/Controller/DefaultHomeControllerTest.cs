﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using sexivirt.UnitTest.Fake;
using sexivirt.UnitTest.Mock.Http;

namespace sexivirt.UnitTest
{
    [TestFixture]
    public class DefaultHomeControllerTest
    {

        [Test]
        public void Index_AskForDefaultPage_GetViewResult()
        {
            var controller = DependencyResolver.Current.GetService<sexivirt.Web.Areas.Default.Controllers.HomeController>();

            var route = new RouteData();

            route.Values.Add("controller", "Home");
            route.Values.Add("action", "Index");
            route.Values.Add("area", "Default");

            var httpContext = new MockHttpContext().Object;
            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            var controllerActionInvoker = new FakeControllerActionInvoker<ViewResult>();
            var result = controllerActionInvoker.InvokeAction(controller.ControllerContext, "Index");
        }

    }
}
