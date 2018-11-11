using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace sexivirt.Web.Global.Config
{
    public class Config : IConfig
    {
        public string ConnectionStrings(string connectionString)
        {
            return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }

        public bool DebugMode
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["DebugMode"]);
            }
        }

        public string AdminEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminEmail"];
            }
        }     
        public string RoboServer
        {
            get
            {
                return ConfigurationManager.AppSettings["RoboServer"];
            }
        }
        public string RoboLogin
        {
            get
            {
                return ConfigurationManager.AppSettings["RoboLogin"];
            }
        }
        public string RoboPass1
        {
            get
            {
                return ConfigurationManager.AppSettings["RoboPass1"];
            }
        }
        public string RoboPass2
        {
            get
            {
                return ConfigurationManager.AppSettings["RoboPass2"];
            }
        }

        public bool EnableMail
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableMail"]);
            }
        }

        public IQueryable<MailTemplate> MailTemplates
        {
            get
            {
                MailTemplateConfig configInfo = (MailTemplateConfig)ConfigurationManager.GetSection("mailTemplateConfig");
                return configInfo.mailTemplates.OfType<MailTemplate>().AsQueryable<MailTemplate>();
            }
        }
    }
}