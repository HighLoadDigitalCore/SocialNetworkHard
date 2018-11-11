using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;


namespace sexivirt.Web.Models.ViewModels
{ 
	public class BlogPostView
    {

        public int ID { get; set; }

        [Required]
		public string Header {get; set; }

        [AllowHtml]
        [Required]
		public string Text {get; set; }

		public string Attach {get; set; }

    }
}