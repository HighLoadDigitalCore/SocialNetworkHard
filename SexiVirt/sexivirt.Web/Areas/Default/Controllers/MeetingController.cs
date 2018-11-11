using System.Web.UI.WebControls.WebParts;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels;
using sexivirt.Web.Models.ViewModels.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class MeetingController : DefaultController
    {
        public ActionResult TodayList()
        {
            var list = Repository.Meetings.Where(x => x.MeetingDate.Date >= DateTime.Now.Date).OrderBy(x => x.MeetingDate).Take(30).ToList();
            return PartialView(list);
        }


        [HttpGet]
        public ActionResult Index(MeetingSearch meetingSearch = null)
        {
            if (!meetingSearch.IsReal)
            {
                meetingSearch = new MeetingSearch();
                if (CurrentUser != null)
                {
                    meetingSearch.Sex = !CurrentUser.Sex;
                    meetingSearch.CityID = CurrentUser.CityID ?? 0;
                    meetingSearch.CityName = CurrentUser.City != null ? CurrentUser.City.Name : "";
                    if (CurrentUser.Birthday.HasValue)
                    {
                        meetingSearch.AgeStart = CurrentUser.Age - 4;
                        meetingSearch.AgeEnd = CurrentUser.Age + 4;
                    }

                    meetingSearch.MinAge = DateTime.Now.Year -
                                           (Repository.Users.Max(x => x.Birthday) ?? DateTime.Now).Year;
                    meetingSearch.MaxAge = DateTime.Now.Year -
                                           (Repository.Users.Min(x => x.Birthday) ?? DateTime.Now).Year;

                    if (meetingSearch.MinAge == 0)
                    {
                        meetingSearch.MinAge = meetingSearch.AgeStart;
                    }
                    if (meetingSearch.MaxAge == 0)
                    {
                        meetingSearch.MaxAge = meetingSearch.AgeEnd;
                    }

                    if (meetingSearch.MaxAge - meetingSearch.AgeEnd > 10)
                    {
                        meetingSearch.MaxAge = meetingSearch.AgeEnd + 10;
                    }
                    if (Math.Abs(meetingSearch.MinAge - meetingSearch.AgeStart) > 10)
                    {
                        meetingSearch.MinAge = meetingSearch.AgeStart - 10;
                    }

                    // if (meetingSearch.MinAge == 0)
                    {
                        meetingSearch.MinAge = 18;
                    }
                    //if (meetingSearch.MaxAge == 0)
                    {
                        meetingSearch.MaxAge = 50;
                    }



                    //peopleSearch.Preferences = CurrentUser.UserPreferences.Select(p => p.Preference).ToList();
                }

            }
            if (meetingSearch.MinAge == 0)
            {
                meetingSearch.MinAge = 18;
            }
            if (meetingSearch.MaxAge == 0)
            {
                meetingSearch.MaxAge = 50;
            }
            var result = SearchMeeting(meetingSearch, 0);
            ViewBag.MeetingSearch = meetingSearch;
            return View(result);
        }


        [HttpPost]
        public ActionResult Index(MeetingSearch meetingSearch = null, FormCollection collection = null)
        {
            UpdateModel(meetingSearch, collection);
            if (!meetingSearch.IsReal)
            {
                meetingSearch = new MeetingSearch();
                if (CurrentUser != null)
                {
                    meetingSearch.Sex = !CurrentUser.Sex;
                    meetingSearch.CityID = CurrentUser.CityID ?? 0;
                    meetingSearch.CityName = CurrentUser.City != null ? CurrentUser.City.Name : "";
                    if (CurrentUser.Birthday.HasValue)
                    {
                        meetingSearch.AgeStart = CurrentUser.Age - 4;
                        meetingSearch.AgeEnd = CurrentUser.Age + 4;
                    }

                    meetingSearch.MinAge = DateTime.Now.Year -
                                           (Repository.Users.Max(x => x.Birthday) ?? DateTime.Now).Year;
                    meetingSearch.MaxAge = DateTime.Now.Year -
                                           (Repository.Users.Min(x => x.Birthday) ?? DateTime.Now).Year;

                    if (meetingSearch.MinAge == 0)
                    {
                        meetingSearch.MinAge = meetingSearch.AgeStart;
                    }
                    if (meetingSearch.MaxAge == 0)
                    {
                        meetingSearch.MaxAge = meetingSearch.AgeEnd;
                    }

                    if (meetingSearch.MaxAge - meetingSearch.AgeEnd > 10)
                    {
                        meetingSearch.MaxAge = meetingSearch.AgeEnd + 10;
                    }
                    if (Math.Abs(meetingSearch.MinAge - meetingSearch.AgeStart) > 10)
                    {
                        meetingSearch.MinAge = meetingSearch.AgeStart - 10;
                    }

                    //if (meetingSearch.MinAge == 0)
                    {
                        meetingSearch.MinAge = 18;
                    }
                    //if (meetingSearch.MaxAge == 0)
                    {
                        meetingSearch.MaxAge = 50;
                    }



                    //peopleSearch.Preferences = CurrentUser.UserPreferences.Select(p => p.Preference).ToList();
                }

            }
            if (meetingSearch.MinAge == 0)
            {
                meetingSearch.MinAge = 18;
            }
            if (meetingSearch.MaxAge == 0)
            {
                meetingSearch.MaxAge = 50;
            }
            var result = SearchMeeting(meetingSearch, 0);
            ViewBag.MeetingSearch = meetingSearch;
            return View(result);
        }

        public ActionResult Load(MeetingSearch meetingSearch, int skip)
        {
            var result = SearchMeeting(meetingSearch, skip);
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        public ActionResult My(int filter = 0)
        {
            var list = CurrentUser.Meetings.AsQueryable();
            if (filter != 0)
            {
                var dateNow = DateTime.Now.Date;
                if (filter == 1)
                {
                    list = list.Where(p => p.MeetingDate >= dateNow);
                }
                else
                {
                    list = list.Where(p => p.MeetingDate < dateNow);
                }
            }
            var result = list.OrderByDescending(p => p.MeetingDate).ToList();
            ViewBag.Filter = filter;
            return View(result);
        }

        private List<Meeting> SearchMeeting(MeetingSearch meetingSearch, int skip = 0)
        {
            var list = Repository.Meetings.Where(p => p.User.Sex == meetingSearch.Sex);

            if (meetingSearch.CityID != 0 && !string.IsNullOrWhiteSpace(meetingSearch.CityName))
            {
                list = list.Where(p => p.CityID == meetingSearch.CityID);
            }

            if (meetingSearch.Preferences != null && meetingSearch.Preferences.Any())
            {
                var preferenceIds = meetingSearch.Preferences.Select(p => p.ID);
                list = list.Where(p => p.User.UserPreferences.Any(r => preferenceIds.Contains(r.PreferenceID)));
            }

            var minBithday = DateTime.Now.AddYears(-meetingSearch.AgeEnd - 1).AddDays(1);
            var maxBithday = DateTime.Now.AddYears(-meetingSearch.AgeStart);
            list = list.Where(p => p.User.Birthday.HasValue
                && p.User.Birthday.Value >= minBithday
                && p.User.Birthday.Value <= maxBithday);

            var startDate = DateTime.Now.AddDays(meetingSearch.DayStart).Date;
            var endDate = DateTime.Now.AddDays(meetingSearch.DayEnd).Date;

            list = list.Where(p => p.MeetingDate >= startDate && p.MeetingDate <= endDate);
            var result = list.OrderBy(p => p.MeetingDate).Skip(skip).Take(18).ToList();
            return result;
        }


        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            var meetingView = new MeetingView();

            return View("Edit", meetingView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var meeting = Repository.Meetings.FirstOrDefault(p => p.ID == id);
            if (meeting != null && meeting.MeetingDate.Date >= DateTime.Now.Date && CurrentUser.ID == meeting.UserID)
            {
                var meetingView = (MeetingView)ModelMapper.Map(meeting, typeof(Meeting), typeof(MeetingView));

                return View(meetingView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var meeting = Repository.Meetings.FirstOrDefault(p => p.ID == id);
            if (meeting != null && meeting.MeetingDate.Date >= DateTime.Now.Date && CurrentUser.ID == meeting.UserID)
            {
                Repository.RemoveMeeting(meeting.ID);
            }
            return RedirectToAction("My");
        }


        [HttpPost]
        [Authorize]
        public ActionResult Edit(MeetingView meetingView)
        {
            if (ModelState.IsValid)
            {
                var meeting = (Meeting)ModelMapper.Map(meetingView, typeof(MeetingView), typeof(Meeting));

                if (meeting.ID == 0)
                {

                    if (CurrentUser.Money >= 50)
                    {
                        meeting.UserID = CurrentUser.ID;
                        var moneyDetail = new MoneyDetail()
                        {
                            Type = (int)MoneyDetail.TypeEnum.PayForMeeting,
                            Sum = -50,
                            Description = "Оплата создания встречи",
                            UserID = CurrentUser.ID
                        };
                        var guid = Guid.NewGuid();
                        Repository.CreateMoneyDetail(moneyDetail, guid);

                        var city =
                            Repository.Cities.FirstOrDefault(x => x.Name.ToLower() == meetingView.CityName.ToLower());
                        if (city == null)
                        {
                            city = new City() { Name = meetingView.CityName };
                            Repository.CreateCity(city);
                        }

                        meeting.CityID = city.ID;
                        Repository.CreateMeeting(meeting);
                        Repository.SubmitMoney(guid);
                    }
                    else
                    {
                        ModelState.AddModelError("Money", "Не достаточно средств на балансе");
                        return View(meetingView);
                    }
                }
                else
                {
                    Repository.UpdateMeeting(meeting);
                }
                return RedirectToAction("My");
            }
            return View(meetingView);
        }

        public ActionResult Popup(int id)
        {
            var meeting = Repository.Meetings.FirstOrDefault(p => p.ID == id);

            if (meeting != null)
            {
                return PartialView(meeting);
            }

            return null;
        }

    }
}