﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using sexivirt.Web.Global.Auth;
using sexivirt.UnitTest.Fake;
using sexivirt.UnitTest.Mock.Http;

namespace sexivirt.UnitTest
{
    [TestFixture]
    public class AdminUserControllerTest
    {

        [Test]
        public void Index_AskForDefaultPage_GetViewResult()
        {
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            var controller = DependencyResolver.Current.GetService<sexivirt.Web.Areas.Admin.Controllers.UserController>();
            auth.Login("admin");

            var route = new RouteData();

            route.Values.Add("controller", "User");
            route.Values.Add("action", "Index");
            route.Values.Add("area", "Admin");

            var values = new FakeValueProvider();

            values["page"] = 2;

            var httpContext = new MockHttpContext(auth).Object;
            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            var controllerActionInvoker = new FakeControllerActionInvoker<ViewResult>(values);
            var result = controllerActionInvoker.InvokeAction(controller.ControllerContext, "Index");
        }

    }
}
