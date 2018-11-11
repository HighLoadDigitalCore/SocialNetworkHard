using sexivirt.Model;
using sexivirt.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class AlbumController : DefaultController
    {
        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                if (CurrentUser != null)
                {
                    return View(CurrentUser);
                }
            }
            else
            {
                var user = Repository.Users.FirstOrDefault(p => p.ID == id);
                return View(user);
            }

            return RedirectToNotFoundPage;
        }

        public ActionResult All()
        {
            if (CurrentUser != null)
            {
                return View(CurrentUser);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Create()
        {
            var albumView = new AlbumView();

            return View("Edit", albumView);
        }

        public ActionResult Delete(int id)
        {
            Repository.DeleteAlbum(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var album = Repository.Albums.FirstOrDefault(p => p.ID == id);

            if (album != null && CurrentUser.ID == album.UserID)
            {
                var albumView = (AlbumView)ModelMapper.Map(album, typeof(Album), typeof(AlbumView));
                return View(albumView);
            }
            return RedirectToNotFoundPage;
        }
        public ActionResult Edit(AlbumView albumView)
        {
            if (albumView.Photos == null || albumView.Photos.Count == 0)
            {
                ModelState.AddModelError("Photos", "Добавьте фото");
            }
            if (ModelState.IsValid)
            {
                var album = (Album)ModelMapper.Map(albumView, typeof(AlbumView), typeof(Album));
                album.UserID = CurrentUser.ID;

                var listPhotos = (List<Photo>)ModelMapper.Map(albumView.Photos, typeof(List<PhotoView>), typeof(List<Photo>));
                if (album.ID == 0)
                {
                    Repository.CreateAlbum(album);
                }
                else
                {
                    Repository.UpdateAlbum(album);
                }

                Repository.UpdateAlbumPhotos(listPhotos, album.ID);
                return RedirectToAction("Index");
            }
            return View(albumView);
        }


        public ActionResult PhotoItem(string filePath)
        {
            var photoView = new PhotoView()
            {
                FilePath = filePath
            };
            return PartialView(photoView);
        }


        public ActionResult Item(int id)
        {
            var album = Repository.Albums.FirstOrDefault(p => p.ID == id);
            if (album != null)
            {
                if (album.HasAccess(CurrentUser))
                {
                    return View(album);
                }
            }
            return RedirectToNotFoundPage;

        }

        [HttpGet]
        public ActionResult BuyAlbum(int id)
        {
            var album = Repository.Albums.FirstOrDefault(p => p.ID == id);
            if (album != null && album.Price > 0)
            {
                return PartialView(album);
            }
            return null;
        }

        public ActionResult Photo(int id)
        {
            var photo = Repository.Photos.FirstOrDefault(p => p.ID == id);
            if (photo != null) 
            {
                return PartialView(photo);
            }
            return null;
        }

        public ActionResult ChangePhoto(int id, bool next = true)
        {
            var photo = Repository.ChangePhoto(id, next);
            if (photo != null)
            {

                return PartialView("Photo", photo);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult AllPhoto(int id)
        {
            var photo = Repository.Photos.FirstOrDefault(p => p.ID == id);
            if (photo != null)
            {
                return PartialView(photo);
            }
            return null;
        }

        public ActionResult ChangeAllPhoto(int id, bool next = true)
        {
            var photo = Repository.ChangeAllPhoto(id, next);
            if (photo != null)
            {

                return PartialView("AllPhoto", photo);
            }
            return RedirectToNotFoundPage;
        }


        [HttpPost]
        public ActionResult BuyAlbum(int id, int userId)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == userId);
            var album = Repository.Albums.FirstOrDefault(p => p.ID == id);

            if (album != null && user != null)
            {
                if (album.HasAccess(user))
                {
                    return Json(new { result = "ok", id = album.ID });
                }

                if (CurrentUser.Money > album.Price)
                {
                    var price = album.Price.Value;
                    var incomePrice = price * 0.8;
                    var feePrice = price * 0.2;

                    var moneyDetail = new MoneyDetail()
                    {
                        UserID = CurrentUser.ID,
                        Sum = -price,
                        Description = string.Format("Оплата за альбом {0} ({1})", album.Name, album.User.FirstName),
                        Type = (int)MoneyDetail.TypeEnum.PayForAlbum,
                    };

                    var incomeMoney = new MoneyDetail()
                    {
                        UserID = album.UserID,
                        Sum = incomePrice,
                        Description = string.Format("Открытие альбома {0} ({1})", album.Name, CurrentUser.FirstName),
                        Type = (int)MoneyDetail.TypeEnum.GetForAlbum,
                    };

                    var feeMoney = new MoneyDetail()
                    {
                        UserID = null,
                        Sum = feePrice,
                        Description = string.Format("Комиссия за открытие альбома {0} ({1}) {2}", album.Name, album.User.FirstName, CurrentUser.FirstName),
                        Type = (int)MoneyDetail.TypeEnum.Fee,
                        IsFee = true,
                    };

                    var guid = Repository.CreateTripleMoneyDetail(moneyDetail, incomeMoney, feeMoney);

                    var access = new AlbumAccess()
                    {
                        AlbumID = album.ID,
                        UserID = user.ID,
                    };
                    Repository.CreateAlbumAccess(access);
                    Repository.SubmitMoney(guid);

                    var feed = new Feed()
                    {
                        ActionType = (int)Feed.ActionTypeEnum.PayForAlbumAccess,
                        AlbumAccessID = access.ID,
                        UserID = album.UserID,
                        ActorID = CurrentUser.ID,
                        IsNew = true,
                        Text = string.Format("Ваш баланс пополнен на {0} р. Поздравляем!", incomeMoney.Sum.ToString("F0"))
                    };
                    Repository.CreateFeed(feed);

                    return Json(new { result = "ok", id = album.ID });
                }
                return Json(new { result = "need-money" });
            }
            return Json(new { result = "ok" });
        }
    }
}