using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using sexivirt.Model;
using sexivirt.Web.Controllers;

namespace sexivirt.Web.Areas.Admin.Controllers
{
    public class ChatController : BaseController
    {
        //
        // GET: /Admin/Chat/
        public ActionResult Index(int? ChatUserID)
        {
            
            return View(ChatUserID);
        }

        public ActionResult SelectUser(string term)
        {
            var users =
                Repository.Users.Where(
                    x =>
                        SqlMethods.Like(x.Login.ToLower(), string.Format("{0}%", term.ToLower())) ||
                        SqlMethods.Like(x.Email.ToLower(), string.Format("{0}%", term.ToLower())) ||
                        SqlMethods.Like(x.FirstName.ToLower(), string.Format("{0}%", term.ToLower()))).ToList();
            return Json(new
            {
                result = "ok",
                data = users.Select(p => new
                {
                    id = p.ID,
                    name = p.FirstName + string.Format(" ({0})", p.Email)
                }),
                term = term
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult StatusList(int? ChatUserID)
        {
                
            

            var users = Repository.ChatUsers.Select(x => x.User.ID).ToList();
            var statusList = new List<KeyValuePair<int, int>>();


            foreach (int user in users)
            {
                var connects =
                    Repository.Connects.Where(
                        p =>
                            p.Messages.Any(
                                r =>
                                    (r.ReceiverID == user && !r.IsSend) ||
                                    (r.SenderID == user && r.IsSend)))
                        .Where(p => p.UserID == user || p.OtherUserID == user);

                
                
                statusList.Add(new KeyValuePair<int, int>(user, connects.ToList().Sum(x => x.UnreadMessagesCount(user))));

            }

            

            return Json(statusList.Select(x => new { uid = x.Key, count = x.Value }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetConnectsList(int? ChatUserID, int count)
        {
            if (!ChatUserID.HasValue)
                return PartialView(new List<Connect>());
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var connects = GetConnects(filter: null, skip: 0, take: count);
            return PartialView(connects);
        }

        public ActionResult ChatUserList()
        {
            return PartialView(Repository.ChatUsers.Select(x => x.User).OrderBy(x=> x.FirstName).ToList());
        }

        [HttpPost]
        public ActionResult AddUser(int? userid)
        {
            if (!userid.HasValue || Repository.ChatUsers.Any(x => x.UserID == userid))
            {
                return PartialView("ChatUserList", Repository.ChatUsers.Select(x => x.User).OrderBy(x => x.FirstName).ToList());
            }
            
            var chatUser = new AdminChat()
            {
                UserID = userid.Value
            };
            Repository.CreateChatUser(chatUser);
            return PartialView("ChatUserList", Repository.ChatUsers.Select(x => x.User).OrderBy(x => x.FirstName).ToList());
        }

        [HttpPost]
        public ActionResult DeleteUser(int? userid)
        {
            if (!userid.HasValue)
            {
                return PartialView("ChatUserList", Repository.ChatUsers.Select(x => x.User).OrderBy(x => x.FirstName).ToList());
            }
            
            Repository.DeleteChatUser(userid.Value);
            return PartialView("ChatUserList", Repository.ChatUsers.Select(x => x.User).OrderBy(x => x.FirstName).ToList());
        }



/**/
/**/
/**/


        public new User CurrentUser { get; set; }

        [Authorize]
        public ActionResult IndexChat(int ChatUserID, string filter = null)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var connects = GetConnects(filter, 0);
            return PartialView(connects);
        }

        public ActionResult LoadConversations(int ChatUserID, string filter = null, int skip = 0)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var result = GetConnects(filter, skip);
            ViewBag.Filter = filter;
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        private List<Connect> GetConnects(string filter = null, int skip = 0, int take = 10)
        {
            var list = Repository.Connects.Where(p => p.Messages.Any(r => (r.ReceiverID == CurrentUser.ID && !r.IsSend) || (r.SenderID == CurrentUser.ID && r.IsSend)));

            if (string.IsNullOrWhiteSpace(filter))
            {
                list = list.Where(p => p.UserID == CurrentUser.ID || p.OtherUserID == CurrentUser.ID);
            }
            else
            {
                list = list.Where(p => (p.UserID == CurrentUser.ID && (p.User.FirstName.ToLower().Contains(filter.ToLower()) || p.User.Email.ToLower().Contains(filter.ToLower())))
                    || (p.OtherUserID == CurrentUser.ID && (p.User1.FirstName.ToLower().Contains(filter.ToLower()) || p.User1.Email.ToLower().Contains(filter.ToLower()))));
            }
            var result = list.ToList().OrderByDescending(p => p.UnreadMessagesCount(CurrentUser.ID)).Skip(skip).Take(take).ToList();
            return result;
        }

        public ActionResult Write(int id, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var connect = Repository.Connects.FirstOrDefault(p => (p.UserID == CurrentUser.ID && p.OtherUserID == user.ID) || (p.UserID == user.ID && p.OtherUserID == CurrentUser.ID));
                if (connect != null)
                {
                    return RedirectToAction("Conversation", new { id = connect.ID });
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
        public ActionResult Conversation(int id, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                var list = GetMessages(connect, 0);
                ViewBag.Connect = connect;
                return PartialView(list);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Load(int id, int skip, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
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
                   .OrderBy/*Descending*/(p => p.AddedDate).DistinctBy(x => x.ID)/*.Skip(skip).Take(10).OrderBy(p => p.AddedDate)*/.ToList();
        }

        public ActionResult GetLast(int id, int? idLastMessage, int? ChatUserID)
        {
            if (!ChatUserID.HasValue)
            {
                return null;
            }
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                if (idLastMessage == null)
                {
                    idLastMessage = 0;
                }
                var list = connect.Messages.Where(p => ((p.ReceiverID == CurrentUser.ID && !p.IsSend) || (p.SenderID == CurrentUser.ID && p.IsSend)) && p.ID > idLastMessage)
                   .OrderBy(p => p.AddedDate).DistinctBy(x => x.ID).ToList();
                return PartialView(list);
            }
            return null;
        }

        public ActionResult WriteMessage(int id, string text, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
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

        public ActionResult ReadAll(int id, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                Repository.ReadMessages(CurrentUser.ID, connect.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveAll(int id, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
            var connect = Repository.Connects.FirstOrDefault(p => p.ID == id);
            if (connect != null)
            {
                Repository.RemoveAllMessages(CurrentUser.ID, connect.ID);
            }
            return new ContentResult();
        }

        public ActionResult RemoveMessage(int id, int ChatUserID)
        {
            CurrentUser = Repository.Users.First(x => x.ID == ChatUserID);
            ViewBag.ChatUserID = ChatUserID;
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