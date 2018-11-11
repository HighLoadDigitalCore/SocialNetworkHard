using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    [Authorize]
    public class FeedController : DefaultController
    {
        //
        // GET: /Default/Feed/
        public ActionResult Index()
        {
            var feeds = Repository.Feeds.Where(p => p.UserID == CurrentUser.ID).OrderByDescending(p => p.ID).Take(18).ToList();

            foreach (var feed in feeds)
            {
                Repository.ReadFeed(feed);
            }
            return View(feeds);
        }

        public ActionResult Load(int skip)
        {
            var result = Repository.Feeds.Where(p => p.UserID == CurrentUser.ID).OrderByDescending(p => p.ID).Skip(skip).Take(18).ToList();

            foreach (var feed in result)
            {
                Repository.ReadFeed(feed);
            }
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        public ActionResult Count()
        {
            var count = Repository.Feeds.Count(p => p.UserID == CurrentUser.ID && p.IsNew);
            return PartialView(count);
        }
	}
}