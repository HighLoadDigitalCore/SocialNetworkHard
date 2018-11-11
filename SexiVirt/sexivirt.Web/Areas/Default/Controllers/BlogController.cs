using System.Collections;
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
    public class BlogController : DefaultController
    {

        public ActionResult Index(int type = 2)
        {
            var users = Repository.Users;
            var blogs = type == 2
                ? users.Where(x => x.BlogPosts.Any()).DistinctBy(x=> x.ID)
                    .OrderByDescending(p => p.BlogPosts.Count).Select(x=> (object)x)
                    /*.SelectMany(x => x.BlogPosts.OrderByDescending(z => z.AddedDate))*/
                : users.OrderByDescending(p => p.AddedDate)
                    .Where(x => x.BlogPosts.Any())
                    .Select(z => z.BlogPosts.OrderByDescending(x => x.AddedDate).First()).Select(x=> (object)x);

           

            ViewBag.Type = type;
            return View(blogs.Take(18).ToList());
        }

        public ActionResult IndexLoad(int skip, int type = 2)
        {
            var result = Repository.Users;
            //result = type == 2 ? result.OrderByDescending(p => p.BlogPosts.Count) : result.OrderByDescending(p => p.AddedDate);
            var blogs = type == 2
                ? result.Where(x => x.BlogPosts.Any()).DistinctBy(x=> x.ID)
                    .OrderByDescending(p => p.BlogPosts.Count).Select(x=> (object)x)/*
                    .SelectMany(x => x.BlogPosts.OrderByDescending(z => z.AddedDate))*/
                : result.OrderByDescending(p => p.AddedDate)
                    .Where(x => x.BlogPosts.Any())
                    .Select(z => z.BlogPosts.OrderByDescending(x => x.AddedDate).First()).Select(x=> (object)x);

            var res = blogs.Skip(skip).Take(18).ToList();
            ViewBag.Total = skip + res.Count;
            ViewBag.Type = type;
            return PartialView(res);
        }

        [Authorize]
        public ActionResult My()
        {
            var blogPosts = Repository.BlogPosts.Where(p => p.UserID == CurrentUser.ID).OrderByDescending(p => p.AddedDate).Take(10).ToList();
            return View(blogPosts);
        }

        [Authorize]
        public ActionResult MyLoad(int skip)
        {
            var blogPosts = Repository.BlogPosts.Where(p => p.UserID == CurrentUser.ID).OrderByDescending(p => p.AddedDate).Skip(skip).Take(10).ToList();
            ViewBag.Total = skip + blogPosts.Count;
            return PartialView(blogPosts);
        }


        public ActionResult Blog(int id)
        {
            if (CurrentUser != null && CurrentUser.ID == id)
            {
                return RedirectToAction("My");
            }
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                ViewBag.User = user;
                var blogPosts = Repository.BlogPosts.Where(p => p.UserID == id).OrderByDescending(p => p.AddedDate).Take(10).ToList();
                return View(blogPosts);
            }

            return RedirectToNotFoundPage;

        }

        public ActionResult BlogLoad(int id, int skip)
        {
            var blogPosts = Repository.BlogPosts.Where(p => p.UserID == id).OrderByDescending(p => p.AddedDate).Skip(skip).Take(10).ToList();
            ViewBag.Total = skip + blogPosts.Count;
            return PartialView(blogPosts);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            var blogPostView = new BlogPostView() {Text = " "};

            return View("Edit", blogPostView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Add");

            var blogPost = Repository.BlogPosts.FirstOrDefault(p => p.ID == id);
            if (blogPost != null && blogPost.UserID == CurrentUser.ID)
            {
                var blogPostView = (BlogPostView)ModelMapper.Map(blogPost, typeof(BlogPost), typeof(BlogPostView));
                return View(blogPostView);
            }

            return RedirectToNotFoundPage;
        }


        [HttpPost]
        public ActionResult Edit(BlogPostView blogPostView)
        {
            if (ModelState.IsValid)
            {
                var blogPost = (BlogPost) ModelMapper.Map(blogPostView, typeof (BlogPostView), typeof (BlogPost));

                blogPost.UserID = CurrentUser.ID;

                if (blogPost.ID == 0)
                {
                    Repository.CreateBlogPost(blogPost);
                }
                else
                {
                    Repository.UpdateBlogPost(blogPost);
                }

                return RedirectToAction("My");
            }
            else if(blogPostView.Text.IsNullOrWhiteSpace())
            {
                ViewBag.TextCSS = "input-validation-error-outer";
            }
            return View(blogPostView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var blogPost = Repository.BlogPosts.FirstOrDefault(p => p.ID == id);
            if (blogPost != null && blogPost.UserID == CurrentUser.ID)
            {
                Repository.RemoveBlogPost(blogPost.ID);
            }
            return RedirectToAction("My");
        }

        public ActionResult Post(int id)
        {
            var blogPost = Repository.BlogPosts.FirstOrDefault(p => p.ID == id);
            if (blogPost != null)
            {
                return View(blogPost);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
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
        public ActionResult CreateComment(CommentView commentView)
        {
            if (ModelState.IsValid)
            {
                var comment = (Comment)ModelMapper.Map(commentView, typeof(CommentView), typeof(Comment));

                comment.UserID = CurrentUser.ID;

                if (comment.ID == 0)
                {
                    Repository.CreateComment(comment);

                    var blogPostComment = new CommentBlogPost()
                    {
                        BlogPostID = commentView.OwnerID,
                        CommentID = comment.ID,
                    };
                    Repository.CreateCommentBlogPost(blogPostComment);
                    if (blogPostComment.BlogPost.UserID != CurrentUser.ID && !comment.ParentID.HasValue)
                    {
                        var feed = new Feed()
                        {
                            ActionType = (int)Feed.ActionTypeEnum.AddBlogPostComment,
                            BlogPostID = blogPostComment.BlogPostID,
                            CommentID = comment.ID,
                            UserID = blogPostComment.BlogPost.UserID,
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
                                BlogPostID = blogPostComment.BlogPostID,
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

         public ActionResult Rating(int id, bool mark)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            if (CurrentUser != null && user != null)
            {
                var entry = Repository.UserRatings.FirstOrDefault(p => p.ReceiverID == id && p.SenderID == CurrentUser.ID);
                if (entry != null)
                {
                    if ((entry.Mark == 1 && mark) || (entry.Mark == -1 && !mark))
                    {
                        return PartialView(user);
                    }
                    else
                    {
                        Repository.RemoveUserRating(entry.ID);
                    }
                }
                else
                {
                    Repository.CreateUserRating(new UserRating
                    {
                        ReceiverID = user.ID,
                        SenderID = CurrentUser.ID,
                        Mark = mark ? 1 : -1
                    });
                }
            }
            var resultUser = Repository.Users.FirstOrDefault(p => p.ID == id);
            return PartialView(resultUser);
        }
    }
}