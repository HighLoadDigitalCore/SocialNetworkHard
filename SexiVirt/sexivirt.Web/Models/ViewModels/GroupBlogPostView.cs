using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;


namespace sexivirt.Web.Models.ViewModels
{ 
	public class GroupBlogPostView
    {
        public int ID { get; set; }

		public int GroupID {get; set; }

        [Required]
		public string Header {get; set; }

        [Required]
		public string Text {get; set; }

		public string Attach {get; set; }

    }
}