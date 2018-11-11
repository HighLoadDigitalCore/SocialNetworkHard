using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sexivirt.Web.Global.Config;
using System.Configuration;

namespace sexivirt.UnitTest.Tools
{
    public class TestConfig : IConfig
    {
        private Configuration configuration;

        public TestConfig(string configPath)
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configPath;
            configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        #region IConfig Members

        public string ConnectionStrings(string connectionString)
        {
            return configuration.ConnectionStrings.ConnectionStrings[connectionString].ConnectionString;
        }

       
        public bool DebugMode
        {
            get
            {
                return bool.Parse(configuration.AppSettings.Settings["DebugMode"].Value);
            }
        }

        public string AdminEmail
        {
            get
            {
                return configuration.AppSettings.Settings["AdminEmail"].Value;
            }
        }

        public string RoboServer { get; private set; }
        public string RoboLogin { get; private set; }
        public string RoboPass1 { get; private set; }
        public string RoboPass2 { get; private set; }

        public bool EnableMail
        {
            get
            {
                return bool.Parse(configuration.AppSettings.Settings["EnableMail"].Value);
            }
        }

        public IQueryable<MailTemplate> MailTemplates
        {
            get
            {
                MailTemplateConfig configInfo = (MailTemplateConfig)configuration.GetSection("mailTemplatesConfig");
                return configInfo.mailTemplates.OfType<MailTemplate>().AsQueryable<MailTemplate>();
            }
        }

        #endregion
    }
}
