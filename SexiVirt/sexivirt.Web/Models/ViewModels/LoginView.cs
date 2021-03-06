﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sexivirt.Web.Models.ViewModels
{
    public class LoginView
    {
        [Required(ErrorMessage = "Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}