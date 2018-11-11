using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;


namespace sexivirt.Web.Models.ViewModels
{
    public class GiftView
    {
        public int ID { get; set; }

        public string Image { get; set; }

        public int Type { get; set; }

        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)Gift.TypeGift.Popular).ToString(), Text = "Популярный", Selected = Type == (int)Gift.TypeGift.Popular };
                yield return new SelectListItem() { Value = ((int)Gift.TypeGift.Sexual).ToString(), Text = "Сексуальный", Selected = Type == (int)Gift.TypeGift.Sexual };
                yield return new SelectListItem() { Value = ((int)Gift.TypeGift.Romantic).ToString(), Text = "Романтичный", Selected = Type == (int)Gift.TypeGift.Romantic };
            }
        }

        public double Price { get; set; }

        public bool IsActive { get; set; }

        public string FullImage
        {
            get
            {
                return string.IsNullOrWhiteSpace(Image) ? "/Content/images/default_gift.png" : Image +"?w=256&h=256&mode=crop&scale=both";
            }
        }
    }
}