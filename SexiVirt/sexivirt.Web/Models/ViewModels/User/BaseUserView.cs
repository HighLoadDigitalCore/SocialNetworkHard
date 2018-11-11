using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using sexivirt.Web.Validation;

namespace sexivirt.Web.Models.ViewModels.User
{
    public class BaseUserView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [ValidEmail(ErrorMessage = "Введите корректный Email")]
        [UserEmailValidation(ErrorMessage = "Email уже зарегистрирован")]
        public string Email { get; set; }
    }
}