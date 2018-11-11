using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sexivirt.Web.Models.ViewModels.User
{
    public class VKSignUpViewModel : BaseUserView
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string FirstName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public DateTime? Birthday { get; set; }

        public bool Sex { get; set; }

        public string AvatarPath { get; set; }

    }
}