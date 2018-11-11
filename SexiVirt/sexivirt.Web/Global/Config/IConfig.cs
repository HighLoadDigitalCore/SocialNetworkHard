using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sexivirt.Web.Global.Config
{
    public interface IConfig
    {
        string ConnectionStrings(string connectionString);

        bool DebugMode { get; }

        string AdminEmail { get; }
        string RoboServer { get; }
        string RoboLogin { get; }
        string RoboPass1 { get; }
        string RoboPass2 { get; }

        bool EnableMail { get; }
      
        IQueryable<MailTemplate> MailTemplates { get; }
    }
}