using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sexivirt.Web.Models.ViewModels
{
    public class PhotoView
    {
        public int ID { get; set; }

        public string FilePath { get; set; }

        public bool IsCover { get; set; }

        public bool Checked { get; set; }
    }
}