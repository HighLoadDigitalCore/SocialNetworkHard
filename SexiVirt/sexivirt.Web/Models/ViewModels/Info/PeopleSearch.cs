using sexivirt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Models.ViewModels.Info
{
    public class PeopleSearch
    {
        public bool IsReal { get; set; }

        public bool Sex { get; set; }

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
                for (int i = MinAge; i <= MaxAge; i++)
                {
                    yield return new SelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = AgeStart == i };
                }
            }
        }

        public IEnumerable<SelectListItem> AgeEndSelectList
        {
            get
            {
                for (int i = MinAge; i <= MaxAge; i++)
                {
                    yield return new SelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = AgeEnd == i };
                }
            }
        }

        public bool WithPhoto { get; set; }

        public bool OnlyOnline { get; set; }

        public bool HasPreference(int idPreference)
        {
            if (Preferences != null)
            {
                return Preferences.Any(p => p.ID == idPreference);
            }
            return false;
        }

        public PeopleSearch()
        {
            AgeStart = MinAge;
            AgeEnd = MaxAge;
        }
    }
}