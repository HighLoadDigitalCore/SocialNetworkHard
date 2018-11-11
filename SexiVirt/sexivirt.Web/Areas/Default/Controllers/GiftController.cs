using System.Web.UI.WebControls.WebParts;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class GiftController : DefaultController
    {
        [Authorize]
        public ActionResult Index(int id = 0)
        {
            ViewBag.GiftUser = id == 0 ? CurrentUser : Repository.Users.FirstOrDefault(p => p.ID == id);
            if (ViewBag.GiftUser == null)
                return RedirectToNotFoundPage;

            var uid = ((User) ViewBag.GiftUser).ID;
            var list = Repository.UserGifts.Where(p => p.ReceiverID == uid).OrderByDescending(p => p.AddedDate).ToList();
            return View(list);
        }

        public ActionResult List(int? type = null)
        {
            var list = Repository.Gifts.Where(p => p.IsActive);
            if (type.HasValue)
            {
                list = list.Where(p => p.Type == type);
            }
            var result = list.ToList();
            return PartialView(result);
        }

        [HttpGet]
        public ActionResult Popup(int id)
        {
            var userGiftView = new UserGiftView()
            {
                ReceiverID = id
            };
            return PartialView(userGiftView);
        }

        [HttpGet]
        public ActionResult Item(int id)
        {
            var userGift = Repository.UserGifts.FirstOrDefault(p => p.ID == id);

            return PartialView(userGift);
        }

        [HttpPost]
        public ActionResult Popup(UserGiftView userGiftView)
        {
            if (ModelState.IsValid)
            {
                var userGift = (UserGift)ModelMapper.Map(userGiftView, typeof(UserGiftView), typeof(UserGift));
                userGift.SenderID = CurrentUser.ID;
                var gift = Repository.Gifts.FirstOrDefault(p => p.ID == userGift.GiftID);
                var receiver = Repository.Users.FirstOrDefault(p => p.ID == userGift.ReceiverID);
                if (gift != null && receiver != null)
                {
                    if (gift.Price > CurrentUser.Money)
                    {
                        ModelState.AddModelError("Money", "Не достаточно средств");
                    }
                    else
                    {
                        var moneyDetail = new MoneyDetail() {
                            Type = (int)MoneyDetail.TypeEnum.PayForGift,
                            Description = "Подарок пользователю " + receiver.FirstName,
                            Sum = -gift.Price,
                            UserID = CurrentUser.ID,
                        };
                        var guid = Guid.NewGuid();
                        Repository.CreateMoneyDetail(moneyDetail, guid);
                        Repository.CreateUserGift(userGift);
                        Repository.SubmitMoney(guid);
                        return PartialView("_Ok");
                    }
                }
            }
            return PartialView(userGiftView);
        }

        [Authorize]
        public ActionResult DeleteUserGift(int id)
        {
            var userGift = Repository.UserGifts.FirstOrDefault(p => p.ID == id);
            if (userGift != null && userGift.ReceiverID == CurrentUser.ID)
            {
                Repository.RemoveUserGift(id);
            }
            return PartialView("_Ok");
        }
	}
}