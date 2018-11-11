using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;


namespace sexivirt.Web.Models.ViewModels
{ 
	public class GroupView
    {

        public int ID { get; set; }

        [Required]
		public string Name {get; set; }

        [Required]
		public string Info {get; set; }

		
		public string AvatarUrl {get; set; }

    }
}