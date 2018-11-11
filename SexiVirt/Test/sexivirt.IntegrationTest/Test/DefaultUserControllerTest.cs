using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Tool;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels.User;
using sexivirt.UnitTest.Mock.Http;
using sexivirt.UnitTest.Tools;

namespace sexivirt.IntegrationTest
{
    [TestFixture]
    public class DefaultUserControllerTest
    {
        [Test]
        public void CreateUser_CreateNormalUser_CountPlusOne()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            var controller = DependencyResolver.Current.GetService<sexivirt.Web.Areas.Default.Controllers.UserController>();

            var countBefore = repository.Users.Count();
            var httpContext = new MockHttpContext().Object;

            var route = new RouteData();

            route.Values.Add("controller", "User");
            route.Values.Add("action", "Register");
            route.Values.Add("area", "Default");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            controller.Session.Add(CaptchaImage.CaptchaValueKey, "1111");

            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
            };

            Validator.ValidateObject<RegisterUserView>(registerUserView);
            controller.Register(registerUserView);

            var countAfter = repository.Users.Count();
            Assert.AreEqual(countBefore + 1, countAfter);
        }
    }
}
