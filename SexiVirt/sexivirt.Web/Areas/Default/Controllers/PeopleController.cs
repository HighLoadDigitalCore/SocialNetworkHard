using sexivirt.Model;
using sexivirt.Web.Models.ViewModels.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class PeopleController : DefaultController
    {
        public ActionResult Index(PeopleSearch peopleSearch = null)
        {
            if (!peopleSearch.IsReal)
            {
                peopleSearch = new PeopleSearch();
                if (CurrentUser != null)
                {
                    peopleSearch.Sex = !CurrentUser.Sex;
                    peopleSearch.CityID = CurrentUser.CityID ?? 0;
                    peopleSearch.CityName = CurrentUser.City != null ? CurrentUser.City.Name : "";
                    if (CurrentUser.Birthday.HasValue)
                    {
                        peopleSearch.AgeStart = CurrentUser.Age - 4;
                        peopleSearch.AgeEnd = CurrentUser.Age + 4;
                    }
                    peopleSearch.MinAge = DateTime.Now.Year -
                                          (Repository.Users.Max(x => x.Birthday) ?? DateTime.Now).Year;
                    peopleSearch.MaxAge = DateTime.Now.Year -
                                           (Repository.Users.Min(x => x.Birthday) ?? DateTime.Now).Year;

                    if (peopleSearch.MinAge == 0)
                    {
                        peopleSearch.MinAge = peopleSearch.AgeStart;
                    }
                    if (peopleSearch.MaxAge == 0)
                    {
                        peopleSearch.MaxAge = peopleSearch.AgeEnd;
                    }

                    if (peopleSearch.MaxAge - peopleSearch.AgeEnd > 10)
                    {
                        peopleSearch.MaxAge = peopleSearch.AgeEnd + 10;
                    }
                    if (Math.Abs(peopleSearch.MinAge - peopleSearch.AgeStart) > 10)
                    {
                        peopleSearch.MinAge = peopleSearch.AgeStart - 10;
                    }

                    //if (peopleSearch.MinAge == 0)
                    {
                        peopleSearch.MinAge = 18;
                    }
                    //if (peopleSearch.MaxAge == 0)
                    {
                        peopleSearch.MaxAge = 50;
                    }


                    //peopleSearch.Preferences = CurrentUser.UserPreferences.Select(p => p.Preference).ToList();
                }
            }
            if (peopleSearch.MinAge == 0)
            {
                peopleSearch.MinAge = 18;
            }
            if (peopleSearch.MaxAge == 0)
            {
                peopleSearch.MaxAge = 50;
            }
            var result = SearchPeople(peopleSearch, 0);
            ViewBag.PeopleSearch = peopleSearch;
            return View(result);
        }

        public ActionResult Load(PeopleSearch peopleSearch, int skip)
        {
            var result = SearchPeople(peopleSearch, skip);
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        private List<Model.User> SearchPeople(PeopleSearch peopleSearch, int skip = 0)
        {
/*
            var adminUsers = Repository.ChatUsers.Select(x=> x.User).ToList();
            foreach (var adminUser in adminUsers)
            {
                
                Repository.VisitUser(adminUser.ID);
            }
*/

            var list = Repository.Users.Where(p => p.Sex == peopleSearch.Sex && p.Birthday != null);

            if (peopleSearch.CityID != 0 && !string.IsNullOrWhiteSpace(peopleSearch.CityName))
            {
                list = list.Where(p => p.CityID == peopleSearch.CityID);
            }

            if (peopleSearch.Preferences != null && peopleSearch.Preferences.Any())
            {
                var preferenceIds = peopleSearch.Preferences.Select(p => p.ID);
                list = list.Where(p => p.UserPreferences.Any(r => preferenceIds.Contains(r.PreferenceID)));
            }

            var minBithday = DateTime.Now.AddYears(-peopleSearch.AgeEnd-1).AddDays(1);
            var maxBithday = DateTime.Now.AddYears(-peopleSearch.AgeStart);
            list = list.Where(p => p.Birthday.HasValue && p.Birthday.Value >= minBithday && p.Birthday.Value <= maxBithday);


            if (peopleSearch.OnlyOnline)
            {
                list = list.Where(x => x.LastVisitDate.AddMinutes(5) > DateTime.Now/* || x.AdminChats.Any()*/);
            }

            var result = list.OrderByDescending(p => p.Rating).Skip(skip).Take(18).ToList();
            return result;
        }

        public ActionResult AjaxPreferences(PeopleSearch peopleSearch)
        {
            var list = Repository.Preferences.OrderBy(p => p.Name).ToList();
            ViewBag.PeopleSearch = peopleSearch;
            return PartialView(list);
        }

        public ActionResult SearchPreference(List<int> preferences)
        {
            if (preferences != null)
            {
                var list = Repository.Preferences.Where(p => preferences.Contains(p.ID)).OrderBy(p => p.Name).ToList();
                return PartialView(list);
            }
            return PartialView(new List<Preference>());
        }

        public ActionResult UserPreference(int id, bool opened)
        {
            ViewBag.Opened = opened;
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                return PartialView(user);
            }
            return null;
        }
    }
}