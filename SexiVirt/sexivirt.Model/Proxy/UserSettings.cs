using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace sexivirt.Model.Proxy
{
    public class UserSettings
    {


        public string MyPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }

        public string MyMail { get; set; }
        public string NewMail { get; set; }
        public string MyPassForMail { get; set; }

    }
}
