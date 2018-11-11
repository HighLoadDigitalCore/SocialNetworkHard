using sexivirt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Models.ViewModels.Info
{
    public class MeetingSearch
    {
        public bool IsReal { get; set; }

        public bool Sex { get; set; }

        public int DayStart { get; set; }

        public int DayEnd { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int AgeStart { get; set; }

        public int AgeEnd { get; set; }


        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        public List<Preference> Preferences { get; set; }

        public IEnumerable<SelectListItem> AgeStartSelectList
        {
            get
            {
                var list = new List<SelectListItem>();
                for (int i = MinAge; i <= MaxAge; i++)
                {
                    list.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = AgeStart == i });
                }
                return list;
            }
        }

        public IEnumerable<SelectListItem> AgeEndSelectList
        {
            get
            {
                var list = new List<SelectListItem>();
                for (int i = MinAge; i <= MaxAge; i++)
                {
                    list.Add(new SelectListItem() {Value = i.ToString(), Text = i.ToString(), Selected = AgeEnd == i});
                }
                return list;
            }
        }

        public IEnumerable<SelectListItem> DayStartSelectList
        {
            get
            {
                for (int i = 0; i <= 31; i++)
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
                    yield return new SelectListItem() 
                    { 
                        Value = i.ToString(),
                        Text = text, 
                        Selected = DayStart == i };
                }
            }
        }

        public IEnumerable<SelectListItem> DayEndSelectList
        {
            get
            {
                for (int i = 0; i <= 31; i++)
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
                    yield return new SelectListItem()
                    {
                        Value = i.ToString(),
                        Text = text,
                        Selected = DayEnd == i
                    };
                }
            }
        }

     
        public bool HasPreference(int idPreference)
        {
            if (Preferences != null)
            {
                return Preferences.Any(p => p.ID == idPreference);
            }
            return false;
        }

        public MeetingSearch()
        {
            AgeStart = MinAge;
            AgeEnd = MaxAge;
            DayStart = 0;
            DayEnd = 7;
        }
    }
}