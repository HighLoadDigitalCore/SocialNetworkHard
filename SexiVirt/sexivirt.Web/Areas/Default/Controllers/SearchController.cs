using sexivirt.Web.Models.ViewModels.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class SearchController : DefaultController
    {
        public ActionResult Index(string searchString)
        {

            var searchInfo = new SearchInfo();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchInfo.SearchAll(searchString);
            }
            return View(searchInfo);
        }

        public ActionResult People(string searchString)
        {
            var searchInfo = new SearchInfo();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchInfo.SearchPeople(searchString);
            }
            return View(searchInfo);
        }

        public ActionResult Events(string searchString)
        {
            var searchInfo = new SearchInfo();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchInfo.SearchEvents(searchString);
            }
            return View(searchInfo);
        }

        public ActionResult Groups(string searchString)
        {
            var searchInfo = new SearchInfo();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchInfo.SearchGroups(searchString);
            }
            return View(searchInfo);
        }

        public ActionResult BlogPosts(string searchString)
        {
            var searchInfo = new SearchInfo();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchInfo.SearchBlogPosts(searchString);
            }
            return View(searchInfo);
        }
    }
}