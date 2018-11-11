using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using sexivirt.Web.Validation;

namespace sexivirt.Web.Models.ViewModels.User
{
    public class ChangePasswordView
    {
        public int ID { get; set; }

        [IsUserPassword(ErrorMessage = "Not valid password")]
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter new password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Enter confirm password")]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}