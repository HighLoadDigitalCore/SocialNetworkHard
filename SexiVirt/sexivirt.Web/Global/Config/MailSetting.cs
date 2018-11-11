using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace sexivirt.Web.Global.Config
{
    public class MailSetting : ConfigurationSection
    {
        [ConfigurationProperty("SmtpReply", IsRequired = true)]
        public string SmtpReply
        {
            get
            {
                return this["SmtpReply"] as string;
            }
            set
            {
                this["SmtpReply"] = value;
            }
        }

        // Create a "SmtpUser" attribute.
        [ConfigurationProperty("SmtpUser", IsRequired = true)]
        public string SmtpUser
        {
            get
            {
                return this["SmtpUser"] as string;
            }
            set
            {
                this["SmtpUser"] = value;
            }
        }
    }
}