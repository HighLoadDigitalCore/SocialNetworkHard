using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;


namespace sexivirt.Web.Models.ViewModels
{
    public class AlbumView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage="Введите наименование")]
        public string Name { get; set; }

        public bool IsPayed { get; set; }

        public double? Price { get; set; }

        public List<PhotoView> Photos { get; set; }
    }
}