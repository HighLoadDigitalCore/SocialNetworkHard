using Microsoft.Ajax.Utilities;
using sexivirt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace sexivirt.Web.Areas.Default.Controllers
{
    public class MessageController : DefaultController
    {
        [Authorize]
        public ActionResult Index(string filter = null)
        {
            return PartialView(GetConnects(filter, 0));
        }

        public ActionResult LoadConversations(string filter = null, int skip = 0)
        {
            var result = GetConnects(filter, skip);
            ViewBag.Filter = filter;
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        private List<Connect> GetConnects(string filter = null, int skip = 0)
        {
            var list = Repository.Connects.Where(p => p.Messages.Any(r => (r.ReceiverID == CurrentUser.ID && !r.IsSend) || (r.SenderID == CurrentUser.ID && r.IsSend)));

            if (string.IsNullOrWhiteSpace(filter)) 
            {
                list = list.Where(p => p.UserID == CurrentUser.ID || p.OtherUserID == CurrentUser.ID);
            } else {
                list = list.Where(p => (p.UserID == CurrentUser.ID && (p.User.FirstName.ToLower().Contains(filter.ToLower()) || p.User.Email.ToLower().Contains(filter.ToLower())))
                    || (p.OtherUserID == CurrentUser.ID && (p.User1.FirstName.ToLower().Contains(filter.ToLower()) || p.User1.Email.ToLower().Contains(filter.ToLower()))));
            }
            var result = list.OrderByDescending(p => p.Messages.OrderByDescending(r => r.AddedDate).FirstOrDefault().AddedDate).Skip(skip).Take(10).ToList();
            return result;
        }

        public ActionResult Write(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null) 
            {
                var connect = Repository.Connects.FirstOrDefault(p => (p.UserID == CurrentUser.ID && p.OtherUserID == user.ID) || (p.UserID == user.ID && p.OtherUserID == CurrentUser.ID));
                if (connect != null)
                {
                    return RedirectToAction("Conversation", new {id = connect.ID});
                }

                connect = new Connect()
                {
                    UserID = CurrentUser.ID,
                    OtherUserID = user.ID
                };
                Repository.CreateConnect(connect);

                return RedirectToAction("Conversation", new { id = connect.ID });
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Conversation(int id)
        {
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                var list = GetMessages(connect, 0);
                ViewBag.Connect = connect;
                return PartialView(list);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Load(int id, int skip)
        {
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                var list = GetMessages(connect, skip);
                if (list.Count > 0)
                {
                    ViewBag.Connect = connect;
                    ViewBag.Total = list.Count + skip;
                    return PartialView(list);
                }
            }
            return null;
        }

        private List<Message> GetMessages(Connect connect, int skip)
        {
            return connect.Messages.Where(p => (p.ReceiverID == CurrentUser.ID && !p.IsSend) || (p.SenderID == CurrentUser.ID && p.IsSend))
                   .OrderBy/*Descending*/(p => p.AddedDate).DistinctBy(x=> x.ID)/*.Skip(skip).Take(10).OrderBy(p => p.AddedDate)*/.ToList();
        }

        public ActionResult GetLast(int id, int? idLastMessage)
        {
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null) 
            {
                if (idLastMessage == null)
                {
                    idLastMessage = 0;
                }
                var list = connect.Messages.Where(p => ((p.ReceiverID == CurrentUser.ID && !p.IsSend) || (p.SenderID == CurrentUser.ID && p.IsSend)) && p.ID > idLastMessage)
                   .OrderBy(p => p.AddedDate).DistinctBy(x=> x.ID).ToList();
                return PartialView(list);
            }
            return null;
        }

        public ActionResult WriteMessage(int id, string text)
        {
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                var opponent = connect.Viral(CurrentUser);
                var message = new Message()
                {
                    SenderID = CurrentUser.ID,
                    ReceiverID = opponent.ID,
                    ConnectID = connect.ID,
                    Text = text,
                };
                Repository.CreateMessage(message);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadAll(int id)
        {
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                Repository.ReadMessages(CurrentUser.ID, connect.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveAll(int id)
        {
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                Repository.RemoveAllMessages(CurrentUser.ID, connect.ID);
            }
            return RedirectToAction("Conversation", new  {id = connect.ID });
        }

        public ActionResult RemoveMessage(int id)
        {
            var message = Repository.Messages.FirstOrDefault(p => p.ID == id);
            if (message != null)
            {
                Repository.RemoveMessage(message.ID);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
	}
}