using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Web.Global.Config;

namespace sexivirt.Web.Mail
{
    public class NotifyMail
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static IConfig _config;

        public static IConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = (DependencyResolver.Current).GetService<IConfig>();

                }
                return _config;
            }
        }

        private static MailSender _mailSender = new MailSender();


        public static void SendNotify(string templateName, string email,
            Func<string, string> subject,
            Func<string, string> body)
        {
            var template = Config.MailTemplates.FirstOrDefault(p => string.Compare(p.Name, templateName, true) == 0);
            if (template != null)
            {
                _mailSender.SendMail(email,
                   subject.Invoke(template.Subject),
                   body.Invoke(template.Template));
            }
            else
            {
                throw new Exception("Can't find template (" + templateName + ")");
            }
        }
    }
}