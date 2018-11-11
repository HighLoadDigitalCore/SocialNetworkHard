using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using sexivirt.Web.Global.Config;

namespace sexivirt.UnitTest
{
    [TestFixture]
    public class MailTemplateTest
    {
        [Test]
        public void MailTemplates_ExistRegisterTemplate_Exist()
        {
            var config = DependencyResolver.Current.GetService<IConfig>();
            var template = config.MailTemplates.FirstOrDefault(p => p.Name.StartsWith("Register"));
            Assert.IsNotNull(template);
        }

        [Test]
        public void MailTemplates_ExistForgotPasswordTemplate_Exist()
        {
            var config = DependencyResolver.Current.GetService<IConfig>();
            var template = config.MailTemplates.FirstOrDefault(p => p.Name.StartsWith("ForgotPassword"));
            Assert.IsNotNull(template);
        }
    }
}
