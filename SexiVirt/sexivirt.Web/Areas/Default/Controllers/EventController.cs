using Microsoft.Ajax.Utilities;
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
    public class EventController : DefaultController
    {
        public ActionResult TodayList()
        {
            var list =
                Repository.Events.Where(p => p.EventDate.Date >= DateTime.Now.Date)

                    .OrderBy(x=> x.EventDate).ThenByDescending(x => x.UserEvents.Count()).Take(30).ToList();
            return PartialView(list);
        }

        public ActionResult Index(EventSearch eventSeach = null)
        {
            if (!eventSeach.IsReal)
            {
                eventSeach = new EventSearch();
            }
            var result = SearchEvent(eventSeach, 0);
            ViewBag.EventSearch = eventSeach;
            return View(result);
        }

        public ActionResult Load(EventSearch eventSeach, int skip)
        {
            var result = SearchEvent(eventSeach, skip);
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        private List<Event> SearchEvent(EventSearch eventSeach, int skip = 0)
        {
            var list = Repository.Events.Where(p => p.EventDate.Date >= DateTime.Now.Date);
            if (eventSeach.DayStart.HasValue)
            {
                var startDate = DateTime.Now.AddDays(eventSeach.DayStart.Value).Date;
                list = list.Where(p => p.EventDate.Date >= startDate.Date);
            }

            if (eventSeach.DayEnd.HasValue)
            {
                var dayEnd = DateTime.Now.AddDays(eventSeach.DayEnd.Value).Date.AddDays(1).AddMilliseconds(-1);
                list = list.Where(p => p.EventDate.Date <= dayEnd.Date);
            }
            var result = list.OrderBy(p => p.EventDate).Skip(skip).Take(18).ToList();
            return result;
        }

        [Authorize]
        public ActionResult My()
        {
            var events = CurrentUser.Events.Concat(CurrentUser.UserEvents.Select(x=> x.Event)).Distinct().OrderBy(x=> x.EventDate.Date < DateTime.Now.Date).ThenBy(p => p.EventDate).ToList();
            return View(events);
        }

        [Authorize]
        public ActionResult Create()
        {
            var eventView = new EventView();

            return View("Edit", eventView);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Create");
            }

            var @event = Repository.Events.FirstOrDefault(p => p.ID == id);

            if (@event != null && CurrentUser.ID == @event.UserID)
            {
                var eventView = ModelMapper.Map(@event, typeof(Event), typeof(EventView));

                return View(eventView);
            }

            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(EventView eventView)
        {
            if (ModelState.IsValid)
            {
                var @event = (Event)ModelMapper.Map(eventView, typeof(EventView), typeof(Event));

                if (!string.IsNullOrEmpty(eventView.CityName))
                {
                    var city =
                        Repository.Cities.FirstOrDefault(p => string.Compare(p.Name, eventView.CityName, true) == 0);
                    if (city != null)
                    {
                        @event.CityID = city.ID;
                    }
                    else
                    {
                        city = new City() {Name = eventView.CityName};
                        Repository.CreateCity(city);
                        @event.CityID = city.ID;
                    }
                }
                if (@event.ID == 0)
                {
                    @event.UserID = CurrentUser.ID;
                    Repository.CreateEvent(@event);
                }
                else
                {
                    Repository.UpdateEvent(@event);
                }
                return RedirectToAction("My");
            }
            else if (eventView.ImagePath.IsNullOrWhiteSpace())
            {
                ViewBag.EventClass = "event-error";
            }
            return View(eventView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var @event = Repository.Events.FirstOrDefault(p => p.ID == id);

            if (@event != null && CurrentUser.ID == @event.UserID)
            {
                Repository.RemoveEvent(@event.ID);
            }
            return RedirectToAction("My");
        }
        public ActionResult Item(int id)
        {
            var @event = Repository.Events.FirstOrDefault(p => p.ID == id);

            if (@event != null)
            {
                return View(@event);
            }

            return RedirectToNotFoundPage;
        }

        public ActionResult LoadUsers(int id)
        {
            var @event = Repository.Events.FirstOrDefault(p => p.ID == id);
            if (@event != null)
            {
                return PartialView(@event);
            }
            return null;
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateComment(int id, int? replyTo)
        {
            var commentView = new CommentView()
            {
                OwnerID = id,
                ParentID = replyTo
            };

            return PartialView(commentView);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(CommentView commentView)
        {
            if (ModelState.IsValid)
            {
                var comment = (Comment)ModelMapper.Map(commentView, typeof(CommentView), typeof(Comment));

                comment.UserID = CurrentUser.ID;

                if (comment.ID == 0)
                {
                    Repository.CreateComment(comment);

                    var eventComment = new CommentEvent()
                    {
                        EventID = commentView.OwnerID,
                        CommentID = comment.ID,
                    };
                    Repository.CreateCommentEvent(eventComment);
                    if (eventComment.Event.UserID != CurrentUser.ID)
                    {
                        var feed = new Feed()
                        {
                            ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                            EventID = eventComment.EventID,
                            CommentID = comment.ID,
                            UserID = eventComment.Event.UserID,
                            ActorID = CurrentUser.ID,
                            IsNew = true,
                        };
                        Repository.CreateFeed(feed);
                    }
                    if (comment.ParentID.HasValue && commentView.CopyID.HasValue)
                    {
                        if (commentView.CopyID != comment.UserID)
                        {
                            var feed = new Feed()
                            {
                                ActionType = (int)Feed.ActionTypeEnum.AddCommentToComment,
                                EventID = eventComment.EventID,
                                CommentID = comment.ID,
                                UserID = commentView.CopyID.Value,
                                ActorID = CurrentUser.ID,
                                IsNew = true,
                            };
                            Repository.CreateFeed(feed);

                        }
                    }
                }
                return PartialView("_Ok");
            }

            return PartialView(commentView);
        }

        [Authorize]
        public ActionResult Enter(int id)
        {
            var @event = Repository.Events.FirstOrDefault(p => p.ID == id);

            if (@event != null)
            {
                if (!CurrentUser.InEvent(@event))
                {
                    Repository.CreateUserEvent(new UserEvent()
                    {
                        EventID = @event.ID,
                        UserID = CurrentUser.ID
                    });
                }
            }
            return RedirectToAction("Item", new { id });
        }

        [Authorize]
        public ActionResult Leave(int id)
        {
            var userEvent = Repository.UserEvents.FirstOrDefault(p => p.UserID == CurrentUser.ID
                && p.EventID == id);

            if (userEvent != null)
            {
                Repository.RemoveUserEvent(userEvent.ID);
            }
            return RedirectToAction("Item", new { id });
        }
    }
}