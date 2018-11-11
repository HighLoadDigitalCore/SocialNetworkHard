using System.Security.Cryptography;
using System.Text;
using Microsoft.Ajax.Utilities;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class MoneyController : DefaultController
    {


        [HttpGet]
        [Authorize]
        public ActionResult MoneyAdd()
        {
            // номер заказа // number of order int nInvId = 0; // описание заказа // order description string sDesc = "Оплата заказа в Тестовом магазине ROBOKASSA"; // сумма заказа // sum of order string sOutSum = "11.00"; // тип товара // code of goods string sShpItem = "1"; // язык // language string sCulture = "ru"; // кодировка // encoding string sEncoding = "utf-8"; // формирование подписи // generate signature string sCrcBase = string.Format("{0}:{1}:{2}:{3}:shpItem={4}", sMrchLogin, sOutSum, nInvId, sMrchPass1, sShpItem); MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); byte[] bSignature = md5.ComputeHash(Encoding.UTF8.GetBytes(sCrcBase)); StringBuilder sbSignature = new StringBuilder(); foreach (byte b in bSignature) sbSignature.AppendFormat("{0:x2}", b); string sCrc = sbSignature.ToString(); // HTML-страница с кассой // ROBOKASSA HTML-page // ltKassa is System.Web.UI.WebControls.Literal; ltKassa.Text = "<script language=JavaScript " + "src=\"https://auth.robokassa.ru/Merchant/PaymentForm/FormMS.js?" + "MerchantLogin=" + sMrchLogin + "&OutSum=" + sOutSum + "&InvoiceID=" + nInvId + "&shpItem=" + sShpItem + "&SignatureValue=" + sCrc + "&Description=" + sDesc + "&Culture=" + sCulture + "&Encoding=" + sEncoding + "\"></script>";

            return PartialView(new MoneyAddModel());
        }

        [HttpGet]
        [Authorize]
        public ActionResult PayMessage()
        {
            var list = Repository.MessagePayments.OrderBy(x => x.Duration).ToList();
            ViewBag.Pay = list.Last().ID;
            // номер заказа // number of order int nInvId = 0; // описание заказа // order description string sDesc = "Оплата заказа в Тестовом магазине ROBOKASSA"; // сумма заказа // sum of order string sOutSum = "11.00"; // тип товара // code of goods string sShpItem = "1"; // язык // language string sCulture = "ru"; // кодировка // encoding string sEncoding = "utf-8"; // формирование подписи // generate signature string sCrcBase = string.Format("{0}:{1}:{2}:{3}:shpItem={4}", sMrchLogin, sOutSum, nInvId, sMrchPass1, sShpItem); MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); byte[] bSignature = md5.ComputeHash(Encoding.UTF8.GetBytes(sCrcBase)); StringBuilder sbSignature = new StringBuilder(); foreach (byte b in bSignature) sbSignature.AppendFormat("{0:x2}", b); string sCrc = sbSignature.ToString(); // HTML-страница с кассой // ROBOKASSA HTML-page // ltKassa is System.Web.UI.WebControls.Literal; ltKassa.Text = "<script language=JavaScript " + "src=\"https://auth.robokassa.ru/Merchant/PaymentForm/FormMS.js?" + "MerchantLogin=" + sMrchLogin + "&OutSum=" + sOutSum + "&InvoiceID=" + nInvId + "&shpItem=" + sShpItem + "&SignatureValue=" + sCrc + "&Description=" + sDesc + "&Culture=" + sCulture + "&Encoding=" + sEncoding + "\"></script>";

            return PartialView(list);
        }

        [HttpPost]
        [Authorize]
        public ActionResult PayMessage(int pay)
        {
            ViewBag.Pay = pay;
            var p = Repository.MessagePayments.FirstOrDefault(x => x.ID == pay);
            if (p == null)
            {
                ViewBag.Error = "Не найден выбранный тип услуги";
            }
            else
            {
                if (p.Price > CurrentUser.Money)
                {
                    ViewBag.Error =
                        "У вас недостаточно средств на счете. <br><a href='#' onclick='showMoneyRefill();'>Пополнить счет</a>";
                }
                else
                {
                    var md = new MoneyDetail()
                    {
                        UserID = CurrentUser.ID,
                        Sum = p.Price * -1,
                        Description = string.Format("Оплата услуги отправки сообщений"),
                        Type = (int)MoneyDetail.TypeEnum.PayForMessage,
                        IsFee = false,
                        Submited = true
                    };
                    var guid = Guid.NewGuid();
                    Repository.CreateMoneyDetail(md, guid);
                    Repository.RecalculateUserMoney(CurrentUser.ID);
                    Repository.UpdateUserPayMessageDate(CurrentUser, p.Duration);
                    ViewBag.Error = "<script type='text/javascript'>document.location.reload();</script>";
                }
            }


            return PartialView(Repository.MessagePayments.OrderBy(x => x.Duration).ToList());
        }
        [HttpPost]
        [Authorize]
        public ActionResult MoneyAdd(MoneyAddModel model)
        {
            // номер заказа // number of order int nInvId = 0; // описание заказа 
            // order description 




            if (model.Sum <= 0)
            {
                model.Error = "Необходимо указать сумму для пополнения";
                return PartialView(model);
            }

            var sDesc = "Пополнение счета на сайте знакомств SexiVirt";
            var sOutSum = model.Sum.ToString("F", new System.Globalization.CultureInfo("en-US"));
            var sShpItem = "2";
            var sCulture = "ru";
            var sEncoding = "utf-8";

            var nInvId = Repository.CreateRoboTransaction(model.Sum, CurrentUser.ID);
            var sCrcBase = string.Format("{0}:{1}:{2}:{3}"/*:shpItem={4}*/, Config.RoboLogin, sOutSum, nInvId, Config.RoboPass1/*, sShpItem*/);

            var md5 = new MD5CryptoServiceProvider();
            var buf = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));
            var sCrc = buf.Select(b => string.Format("{0:x2}", b)).Aggregate((acc, el) => acc + el);



            /*
                        string url = HttpUtility.UrlPathEncode(Config.RoboServer + "?" + "MrchLogin=" + Config.RoboLogin +
                                     "&OutSum=" + sOutSum + "&InvId=" + nInvId + "&shpItem=" + sShpItem + "&SignatureValue=" +
                                     sCrc + "&Desc=" + sDesc + "&Culture=" + sCulture + "&Encoding=" + sEncoding);
            */
            string url = HttpUtility.UrlPathEncode(Config.RoboServer + "?" + "MrchLogin=" + Config.RoboLogin +
                         "&OutSum=" + sOutSum + "&InvId=" + nInvId + "&SignatureValue=" +
                         sCrc + "&Desc=" + sDesc);

            model.Error = "";
            model.RedirectLink = url;

            return PartialView(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult MoneyRemove()
        {
            return PartialView(new MoneyWithdrawModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult MoneyRemove(MoneyWithdrawModel model)
        {
            int sum = 0;
            int.TryParse(model.PaySum, out sum);

            var msg = new List<string>();
            if (model.PayType.IsNullOrWhiteSpace())
            {
                msg.Add("Необходимо указать способ вывода средств (Yandex или QIWI)");
            }
            if (model.PayAccount.IsNullOrWhiteSpace())
            {
                msg.Add("Необходимо указать номер счета");
            }
            if (model.PaySum.IsNullOrWhiteSpace() || sum == 0)
            {
                msg.Add("Необходимо указать сумму для вывода");
            }
            Repository.RecalculateUserMoney(CurrentUser.ID);
            if (CurrentUser.Money < sum)
            {
                msg.Add("У вас недостаточно денег на счете");
            }
            if (msg.Any())
            {
                ViewBag.Message = string.Join("<br>", msg.ToArray());
            }
            else
            {

                var md = new MoneyDetail()
                {
                    UserID = CurrentUser.ID,
                    Sum = sum * -1,
                    Description = string.Format("Вывод средств"),
                    Type = (int)MoneyDetail.TypeEnum.Withdraw,
                    IsFee = false,
                    Submited = false
                };
                var guid = Guid.NewGuid();
                Repository.CreateMoneyDetail(md, guid);


                var w = new MoneyWithdraw()
                {
                    UserID = CurrentUser.ID,
                    Account = model.PayAccount,
                    AddedDate = DateTime.Now,
                    Provider = model.PayType == "Yandex" ? 0 : 1,
                    Submitted = false,
                    MoneyDetailID = md.ID,
                    Sum = sum
                };
                Repository.CreateMoneyWithdraw(w);
                ViewBag.Message = "Ваш запрос отправлен администратору сайта. <br>Средства будут списаны со счета после подтвержения администратором.";
            }
            return PartialView(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult MoneyHistory()
        {
            var list = Repository.MoneyDetails.Where(x => x.UserID == CurrentUser.ID)
                .OrderByDescending(x => x.AddedDate).ToList();
            return PartialView(list);
        }

        [HttpGet]
        [Authorize]

        public ActionResult Popup()
        {
            var depositCandidateView = new DepositCandidateView();
            return PartialView(depositCandidateView);
        }
    }
}