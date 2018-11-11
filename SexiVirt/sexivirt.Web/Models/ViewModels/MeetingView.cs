using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;


namespace sexivirt.Web.Models.ViewModels
{
    public class MeetingView
    {

        public int ID { get; set; }

        public int CityID { get; set; }

        [Required]
        public string CityName { get; set; }

        public DateTime MeetingDate { get; set; }


        public IEnumerable<SelectListItem> MeetingDateSelectList
        {
            get
            {
                for (int i = 0; i < 30; i++)
                {
                    string text = null;
                    if (i == 0)
                    {
                        text = "Сегодня";
                    }
                    else if (i == 1)
                    {
                        text = "Завтра";
                    }
                    else if (i == 2)
                    {
                        text = "Послезавтра";
                    }
                    else
                    {
                        text = DateTime.Now.AddDays(i).ToString("dd MMMM");
                    }
                    var dateTime = DateTime.Now.AddDays(i).ToString();
                    yield return new SelectListItem()
                    {
                        Value = dateTime,
                        Text = text,
                        Selected = MeetingDate.Date == DateTime.Now.AddDays(i).Date
                    };
                }
            }
        }

        [Required]
        public string Text { get; set; }

    }
}