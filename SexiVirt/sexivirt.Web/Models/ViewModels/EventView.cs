using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;


namespace sexivirt.Web.Models.ViewModels
{
    public class EventView
    {

        public int ID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public DateTime? EventDate { get; set; }

        /*[Required]*/
        public string CityName { get; set; }

        [Required]
        public string Place { get; set; }

        public string Coordinate { get; set; }
    }
}