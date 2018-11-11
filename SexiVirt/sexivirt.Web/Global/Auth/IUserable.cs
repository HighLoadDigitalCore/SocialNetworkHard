using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using sexivirt.Model;

namespace sexivirt.Web.Global.Auth
{
    public interface IUserable : IIdentity
    {
        User User { get; }
    }
}