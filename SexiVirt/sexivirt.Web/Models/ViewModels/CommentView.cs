using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;


namespace sexivirt.Web.Models.ViewModels
{ 
	public class CommentView
    {
        public int _ID { 
            get 
            {
                return ID;
           }
            set
            {
                ID = value;
            }
        }

        public int? CopyID { get; set; }

        public int ID { get; set; }

        public int OwnerID { get; set; }

		public int? ParentID {get; set; }

        [Required]
        public string Text {get; set; }


    }
}