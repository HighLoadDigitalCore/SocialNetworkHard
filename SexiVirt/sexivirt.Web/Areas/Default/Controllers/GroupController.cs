using Microsoft.Ajax.Utilities;
using sexivirt.Model;
using sexivirt.Web.Global;
using sexivirt.Web.Models.ViewModels;
using sexivirt.Web.Models.ViewModels.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class GroupController : DefaultController
    {
        [HttpGet]
        [Authorize]
        public ActionResult PostEdit(int ID)
        {
            var post = Repository.GroupBlogPosts.FirstOrDefault(x => x.ID == ID);
            return View(post);
        }
        [HttpPost]
        [Authorize]
        public ActionResult PostEdit(GroupBlogPost model)
        {
            ViewBag.IsPost = true;

            var fromDB = Repository.GroupBlogPosts.FirstOrDefault(x => x.ID == model.ID);
            if (fromDB == null)
            {
                return RedirectToAction("Index", "Group");
            }

            fromDB.Header = model.Header;
            fromDB.Text = model.Text;
            fromDB.Attach = model.Attach;

            if (fromDB.Header.IsNullOrWhiteSpace() || fromDB.Text.IsNullOrWhiteSpace())
                return View(fromDB);

            Repository.UpdateGroupBlogPost(fromDB);
            return RedirectToAction("Item", "Group", new {ID = fromDB.GroupID});

        }
        public ActionResult Index(GroupSearch groupSeach = null)
        {
            if (!groupSeach.IsReal)
            {
                groupSeach = new GroupSearch();
            }
            var result = SearchGroup(groupSeach, 0);
            ViewBag.GroupSearch = groupSeach;
            return View(result);
        }

        public ActionResult Load(GroupSearch groupSeach, int skip)
        {
            var result = SearchGroup(groupSeach, skip);
            ViewBag.Total = skip + result.Count;
            return PartialView(result);
        }

        public ActionResult LoadPosts(int id, int skip)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null)
            {
                var result = group.SubBlogPost(skip).Take(10).ToList();
                ViewBag.Total = skip + result.Count;
                return PartialView(result);
            }
            return null;
        }

        private List<Group> SearchGroup(GroupSearch groupSeach, int skip = 0)
        {
            var list = Repository.Groups.AsEnumerable();

            if (!string.IsNullOrEmpty(groupSeach.SearchString))
            {
                list = SearchEngine.SearchAll(groupSeach.SearchString, list);
            }
            var result = list.OrderByDescending(p => p.Rating).Skip(skip).Take(18).ToList();
            return result;
        }

        public ActionResult My()
        {
            return View(CurrentUser);
        }

        public ActionResult Create()
        {
            var groupView = new GroupView();

            return View("Edit", groupView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null && CurrentUser.ID == group.UserID)
            {
                var groupView = ModelMapper.Map(group, typeof(Group), typeof(GroupView));

                return View(groupView);
            }

            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(GroupView groupView)
        {

            if (ModelState.IsValid)
            {
                var group = (Group)ModelMapper.Map(groupView, typeof(GroupView), typeof(Group));
                if (group.ID == 0)
                {
                    group.UserID = CurrentUser.ID;
                    Repository.CreateGroup(group);
                    if (!CurrentUser.InGroup(group))
                    {
                        Repository.CreateUserGroup(new UserGroup()
                        {
                            GroupID = group.ID,
                            UserID = CurrentUser.ID
                        });
                    }
                }
                else
                {
                    Repository.UpdateGroup(group);
                }
                return RedirectToAction("My");
            }
            return View(groupView);
        }

        public ActionResult Item(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null)
            {
                return View(group);
            }

            return RedirectToNotFoundPage;
        }

        public ActionResult LoadUsers(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                return PartialView(group);
            }
            return null;
        }        
        public ActionResult HideUsers(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                return PartialView(group);
            }
            return null;
        }

        [HttpGet]
        public ActionResult CreatePost(int id)
        {
            var groupBlogPostView = new GroupBlogPostView()
            {
                GroupID = id
            };
            return PartialView(groupBlogPostView);
        }


        [HttpPost]
        public ActionResult CreatePost(GroupBlogPostView groupBlogPostView)
        {
            if (ModelState.IsValid)
            {
                var groupBlogPost = (GroupBlogPost)ModelMapper.Map(groupBlogPostView, typeof(GroupBlogPostView), typeof(GroupBlogPost));
                groupBlogPost.UserID = CurrentUser.ID;
                Repository.CreateGroupBlogPost(groupBlogPost);
                return PartialView("_Ok");
            }
            return PartialView(groupBlogPostView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteGroup(int id)
        {
            var groupBlogPost = Repository.GroupBlogPosts.FirstOrDefault(p => p.ID == id);
            if (groupBlogPost != null && groupBlogPost.UserID == CurrentUser.ID)
            {
                Repository.RemoveBlogPost(groupBlogPost.ID);
            }
            return PartialView("_Ok");
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null && group.UserID == CurrentUser.ID)
            {
                Repository.RemoveGroup(group.ID);
            }
            return RedirectToAction("My");
        }

        public ActionResult Post(int id)
        {
            var groupBlogPost = Repository.GroupBlogPosts.FirstOrDefault(p => p.ID == id);
            if (groupBlogPost != null)
            {
                return View(groupBlogPost);
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

                    var commentGroupBlogPost = new CommentGroupBlogPost()
                    {
                        GroupBlogPostID = commentView.OwnerID,
                        CommentID = comment.ID,
                    };
                    Repository.CreateCommentGroupBlogPost(commentGroupBlogPost);
                    if (commentGroupBlogPost.GroupBlogPost.UserID != CurrentUser.ID)
                    {
                        var feed = new Feed()
                        {
                            ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                            GroupBlogPostID = commentGroupBlogPost.GroupBlogPost.ID,
                            CommentID = comment.ID,
                            UserID = commentGroupBlogPost.GroupBlogPost.UserID,
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
                                GroupBlogPostID = commentGroupBlogPost.GroupBlogPost.ID,
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

        public ActionResult EnterGroup(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null)
            {
                if (!CurrentUser.InGroup(group))
                {
                    Repository.CreateUserGroup(new UserGroup()
                    {
                        GroupID = group.ID,
                        UserID = CurrentUser.ID
                    });
                }
            }
            return RedirectToAction("Item", new { id });
        }

        public ActionResult LeaveGroup(int id)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.UserID == CurrentUser.ID && p.GroupID == id);

            if (userGroup != null)
            {
                Repository.RemoveUserGroup(userGroup.ID);
            }
            return RedirectToAction("Item", new { id });
        }
    }
}