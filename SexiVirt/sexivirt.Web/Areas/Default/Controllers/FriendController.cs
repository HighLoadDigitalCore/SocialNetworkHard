using System.Web.UI.WebControls.WebParts;
using sexivirt.Model;
using sexivirt.Web.Global;
using sexivirt.Web.Models.ViewModels.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class FriendController : DefaultController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [Authorize]
        public ActionResult Index(ContactSearch contactSearch = null)
        {
            var newFriends = CurrentUser.PassiveFriendships.Where(p => !p.Approved);
            ViewBag.ContactSearch = contactSearch;
            return View(newFriends.ToList());
        }

        public ActionResult NewContacts(ContactSearch contactSearch = null)
        {
            
            var list = CurrentUser.PassiveFriendships.Where(p => !p.Approved);
            if (!string.IsNullOrEmpty(contactSearch.SearchString))
            {
                list = SearchEngine.SearchNewContacts(contactSearch.SearchString, list);
            }
            return PartialView(list.ToList());
        }

        public ActionResult MyContacts(ContactSearch contactSearch = null)
        {
            var result = SearchMyContact(contactSearch, 0);
            return PartialView(result);
        }

        public ActionResult Load(ContactSearch contactSearch, int skip)
        {
            var result = SearchMyContact(contactSearch, skip);
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        private List<Friendship> SearchMyContact(ContactSearch contactSearch, int skip = 0)
        {
            var list = CurrentUser.ActiveFriendships.Where(p => p.Approved);
            if (!string.IsNullOrEmpty(contactSearch.SearchString))
            {
                list = SearchEngine.Search(contactSearch.SearchString, list);
            }
          
            var result = list.OrderByDescending(p => p.ID).Skip(skip).Take(18).ToList();
            return result;
        }

        public ActionResult InviteToFriend(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            if (user.AskForFriend(CurrentUser.ID))
            {
                var friendship = user.ActiveFriendships.FirstOrDefault(p => p.ReceiverID == CurrentUser.ID);
                if (friendship != null)
                {
                    Repository.ConfirmFriendship(friendship.ID);
                }
            }
            else
            {
                if (!CurrentUser.AskForFriend(user.ID) && !user.HasFriend(CurrentUser.ID))
                {
                    var friendship = new Friendship()
                    {
                        SenderID = CurrentUser.ID,
                        ReceiverID = user.ID
                    };
                    Repository.CreateFriendship(friendship);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveFriend(int id)
        {
            var user = Repository.Users.Where(p => p.ID == id).FirstOrDefault();
            if (user != null)
            {
                Repository.RemoveFriend(CurrentUser.ID, user.ID);
                if (Request.UrlReferrer != null)
                {
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ConfirmFriendship(int id)
        {
            Repository.ConfirmFriendship(id);

            var friendship = Repository.Friendships.FirstOrDefault(p => p.ID == id);

            var feed = new Feed()
            {
                ActionType = (int)Feed.ActionTypeEnum.AddFriend,
                UserID = friendship.SenderID,
                ActorID = friendship.ReceiverID,
                IsNew = true,
            };
            Repository.CreateFeed(feed);
            return PartialView("_Ok");
            //return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DeclineFriendship(int id)
        {
            Repository.DeclineFriendship(id);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddContact(int id)
        {
            var friendship = CurrentUser.PassiveFriendships.FirstOrDefault(p => p.ID == id);
            if (friendship != null)
            {
                var reverseFriendship = Repository.Friendships.FirstOrDefault(p => 
                    p.SenderID == friendship.ReceiverID
                    && p.ReceiverID ==friendship.SenderID);
                if (reverseFriendship != null)
                {
                    return PartialView("ShortFullContactItem", reverseFriendship);
                }
            }
            return null;
        }

        public ActionResult BlockUser(int id)
        {
            Repository.BlockUser(CurrentUser.ID, id);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnblockUser(int id)
        {
            Repository.UnblockUser(CurrentUser.ID, id);

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}
