using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using sexivirt.Web.Global.Config;

namespace sexivirt.Web.Mail
{
    public class MailSender
    {

        protected IConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = DependencyResolver.Current.GetService<IConfig>();
                }
                return _config;
            }
        }
        private IConfig _config;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void SendMail(string email, string subject, string body, MailAddress mailAddress = null)
        {
            try
            {
                if (Config.EnableMail)
                {
                    MailMessage message = new MailMessage(Config.AdminEmail, email)
                    {
                        Subject = subject,
                        BodyEncoding = Encoding.UTF8,
                        Body = body,
                        IsBodyHtml = true,
                        SubjectEncoding = Encoding.UTF8,
                        
                        
                    };
                    
                    SmtpClient client = new SmtpClient();
                    client.Send(message);
                }
                else
                {
                    logger.Debug("Email : {0} {1} \t Subject: {2} {3} Body: {4}", email, Environment.NewLine, subject, Environment.NewLine, body);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Mail send exception", ex.Message);
            }
        }
    }

}