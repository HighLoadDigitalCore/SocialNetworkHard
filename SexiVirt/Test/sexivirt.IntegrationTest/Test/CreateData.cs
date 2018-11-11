using GenerateData;
using NUnit.Framework;
using sexivirt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tool;
using sexivirt.IntegrationTest.Tools;

namespace sexivirt.IntegrationTest
{
    [TestFixture]
    public class CreateData
    {
        private static Random rand = new Random((int)DateTime.Now.Ticks);

        [Test]
        public void GenerateData()
        {
            //GeneratePreferences(30);
            //GenerateGifts(50);
            //for (int i = 0; i < 10; i++)
            //{
            //    try
            //    {
            //        GenerateUsers(10);
            //        GenerateMoneyDetails(10);
            //        GenerateAlbums(10);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error: " + ex.Message);
            //    }
            //}

            //GenerateMessages(50, 20);

            //GenerateFriendship(2, 20, 100);
            //GenerateMakeGifts();
            //GenerateBlogPosts(20);
            //GenerateBlogPostsComments(10);

            /*GenerateMeetings(50, 3);
            GenerateEvents(100, 5);
            GenerateEventComments();*/

            //GenerateGroups(10, 10);

            //GenerateUserGroups();
            //GenerateGroupBlogPosts(20, 10, 3);

            GenerateFeeds();
        }

        public void GeneratePreferences(int count = 10)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            for (int i = 0; i < count; i++)
            {
                var preference = new Preference()
                {
                    Name = Team.GetRandomSubjective()
                };
                repository.CreatePreference(preference);

                Console.WriteLine("Создано предпочтение : " + preference.Name);
            }
        }

        public void GenerateGifts(int count = 10)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            for (int i = 0; i < count; i++)
            {
                var gift = new Gift()
                {
                    Image = Imaginarium.SaveRandomImage("/Content/files/gifts/"),
                    Type = rand.Next(100) % 3 + 1,
                    Price = rand.Next(10) * 100,
                    IsActive = true,
                };
                repository.CreateGift(gift);
                Console.WriteLine("Создан подарок за " + gift.Price.ToString() + " рублей");
            }
        }

        public void GenerateUsers(int count = 10)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            var cities = repository.Cities.ToList();

            for (int i = 0; i < count; i++)
            {
                var city = cities.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                var firstName = Name.GetRandom();
                var lastName = Surname.GetRandom();

                var user = new User()
                {
                    CityID = city.ID,
                    FirstName = string.Format("{0} {1}", firstName, lastName),
                    Password = "123456",
                    Description = Textarium.GetRandomText(1).Teaser(250),
                    Birthday = GetRandomBirthDay(18, 75),
                    AvatarPath = Imaginarium.SaveRandomImage("/Content/files/avatars/"),
                    Sex = rand.Next(2) == 1,
                    Height = rand.Next(30) + 160,
                    Weight = rand.Next(40) + 50,
                    Rating = rand.Next(1000)
                };
                user.Email = Email.GetRandom(firstName, lastName);
                user.Login = user.Email;
                repository.CreateUser(user);
                Console.WriteLine("Создан человечек : " + user.FirstName);
                var status = Textarium.GetRandomText(1).Teaser(120, "...");
                var userStatus = new UserStatus()
                {
                    UserID = user.ID,
                    Text = status,
                };
                repository.CreateUserStatus(userStatus);
                Console.WriteLine("Человечек " + user.FirstName + " сделал статус: " + userStatus.Text);

                var max = rand.Next(3) + 3;
                var preferences = repository.Preferences.ToList();
                for (int j = 0; j < max; j++)
                {
                    var preference = preferences.OrderBy(p => Guid.NewGuid()).First();
                    var userPreference = new UserPreference()
                    {
                        UserID = user.ID,
                        PreferenceID = preference.ID
                    };
                    repository.CreateUserPreference(userPreference);
                    Console.WriteLine("Человечек " + user.FirstName + " любит : " + preference.Name);
                }
            }
            Assert.AreEqual(0, 0);
        }


        public void GenerateMoneyDetails(int count = 10)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            var users = repository.Users.OrderByDescending(p => p.ID).Take(count);

            var income = rand.Next(10) * 1000 + 2000;

            foreach (var user in users)
            {
                var moneyIncome = new MoneyDetail()
                {
                    UserID = user.ID,
                    Sum = income,
                    Type = (int)MoneyDetail.TypeEnum.Income,
                    Description = "Пополнение на старте",
                };
                var guid = Guid.NewGuid();
                repository.CreateMoneyDetail(moneyIncome, guid);
                repository.SubmitMoney(guid);
            }
            Assert.AreEqual(0, 0);
        }

        public void GenerateAlbums(int count = 10)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            var users = repository.Users.OrderByDescending(p => p.ID).Take(count);
            foreach (var user in users)
            {
                var countAlbums = rand.Next(2) + 1;
                for (int i = 0; i < countAlbums; i++)
                {
                    var album = new Album()
                    {
                        UserID = user.ID,
                        Name = Name.GetRandom(),
                        Price = rand.Next(10) % 2 == 0 ? (int?)(rand.Next(20) * 100) : null
                    };

                    repository.CreateAlbum(album);
                    var photosCount = rand.Next(3) + 4;

                    for (int j = 0; j < photosCount; j++)
                    {
                        var photo = new Photo()
                        {
                            AlbumID = album.ID,
                            FilePath = Imaginarium.SaveRandomImage("/Content/files/uploads/", 800, 40),
                        };
                        repository.CreatePhoto(photo);
                    }
                }
            }
            Assert.AreEqual(0, 0);
        }

        public void GenerateMakeGifts()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            var users = repository.Users.OrderByDescending(p => p.ID).ToList();
            var gifts = repository.Gifts.ToList();
            foreach (var user in users)
            {
                var countGifts = rand.Next(2) + 1;
                for (int i = 0; i < countGifts; i++)
                {
                    var gift = gifts.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                    var otherUser = users.Where(p => p.ID != user.ID).OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                    var userGift = new UserGift()
                    {
                        SenderID = user.ID,
                        ReceiverID = otherUser.ID,
                        GiftID = gift.ID,
                        Text = Textarium.GetRandomText(2),
                        Visible = rand.Next(100) % 2 == 0
                    };

                    repository.CreateUserGift(userGift);


                    Console.WriteLine("Человечек " + user.FirstName + " подарил подарок : " + otherUser.FirstName);
                }
            }
            Assert.AreEqual(0, 0);
        }

        public DateTime GetRandomBirthDay(int startAge, int endAge)
        {
            var min = DateTime.Now.AddYears(-endAge);
            var max = DateTime.Now.AddYears(-startAge);

            var countDays = (int)((max - min).TotalDays);

            var randDay = rand.Next(countDays);

            return min.AddDays(randDay);
        }

        public void GenerateMessages(int count = 10, int messageCount = 100)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.OrderBy(p => p.ID).ToList();
            var me = repository.Users.FirstOrDefault(p => p.ID == 2);
            for (int i = 0; i < count; i++)
            {
                var talkUsers = users.OrderBy(p => Guid.NewGuid()).Take(2).ToList();
                var first = me;
                var second = me.ID == talkUsers[0].ID ? talkUsers[1] : talkUsers[0];

                var connect = repository.Connects.FirstOrDefault(p => (p.UserID == first.ID && p.OtherUserID == second.ID) || (p.OtherUserID == first.ID && p.UserID == second.ID));
                if (connect == null)
                {
                    connect = new Connect()
                    {
                        UserID = first.ID,
                        OtherUserID = second.ID
                    };
                    repository.CreateConnect(connect);
                }
                var currentDate = DateTime.Now.AddDays(-(rand.Next(100)));
                for (int j = 0; j < messageCount; j++)
                {
                    currentDate = currentDate.AddMinutes(rand.Next(100));
                    var message = new Message()
                    {
                        SenderID = rand.Next(10) % 2 == 0 ? first.ID : second.ID,
                        AddedDate = currentDate,
                        Text = Textarium.GetRandomText(2).Teaser(rand.Next(800) + 100, "")
                    };

                    message.ReceiverID = first.ID == message.SenderID ? second.ID : first.ID;

                    repository.CreateMessage(message);

                    Console.WriteLine(string.Format("{0} написал {1} ({2}...)", message.Sender.FirstName, message.Receiver.FirstName, message.Text.Teaser(10)));
                }
            }
        }

        public void GenerateFriendship(int userId, int countNew, int countApprowed)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var user = repository.Users.FirstOrDefault(p => p.ID == userId);

            if (user != null)
            {
                var users = repository.Users.Where(p => p.ID != userId).Take(countNew);
                foreach (var newUser in users)
                {
                    var friendship = new Friendship()
                    {
                        ReceiverID = user.ID,
                        SenderID = newUser.ID
                    };
                    repository.CreateFriendship(friendship);
                }
                var myFriends = repository.Users.Where(p => p.ID != userId).Skip(countNew).Take(countApprowed);
                foreach (var friendUser in myFriends)
                {
                    var friendship = new Friendship();

                    if (rand.Next() % 3 == 0)
                    {
                        friendship.ReceiverID = friendUser.ID;
                        friendship.SenderID = user.ID;
                    }
                    else
                    {
                        friendship.ReceiverID = user.ID;
                        friendship.SenderID = friendUser.ID;
                    }
                    repository.CreateFriendship(friendship);
                    repository.ConfirmFriendship(friendship.ID);
                    var feed = new Feed()
                    {
                        ActionType = (int)Feed.ActionTypeEnum.AddFriend,
                        UserID = friendship.SenderID,
                        ActorID = friendship.ReceiverID,
                        IsNew = true,
                    };
                    repository.CreateFeed(feed);
                }
            }
        }
        public void GenerateBlogPosts(int count)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.ToList();

            foreach (var user in users)
            {
                var countNew = count - rand.Next(20) + 10;
                for (int i = 0; i < countNew; i++)
                {
                    var blogPost = new BlogPost()
                    {
                        UserID = user.ID,
                        Text = Textarium.GetRandomHtmlText(1 + rand.Next(3)),
                        Attach = rand.Next(12) % 4 == 0 ? Imaginarium.SaveRandomImage("/Content/files/uploads/") : null,
                        Header = Team.GetRandom() + " " + Team.GetRandom(),
                    };

                    repository.CreateBlogPost(blogPost);

                    Console.WriteLine(string.Format("{0} написал в блоге {1}", user.FirstName, blogPost.Header));
                }
            }
        }

        public void GenerateBlogPostsComments(int count)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.ToList();

            var blogPosts = repository.BlogPosts.ToList();

            foreach (var blogPost in blogPosts)
            {
                var countNew = count - rand.Next(20) + 10;
                for (int i = 0; i < countNew; i++)
                {
                    var user = users.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                    var comment = new Comment()
                    {
                        UserID = user.ID,
                        Text = Textarium.GetRandomText(1 + rand.Next(2)),
                    };
                    repository.CreateComment(comment);
                    var blogComment = new CommentBlogPost()
                    {
                        CommentID = comment.ID,
                        BlogPostID = blogPost.ID
                    };
                    repository.CreateCommentBlogPost(blogComment);
                    if (blogComment.BlogPost.UserID != user.ID)
                    {
                        var feed = new Feed()
                        {
                            ActionType = (int)Feed.ActionTypeEnum.AddBlogPostComment,
                            BlogPostID = blogComment.BlogPostID,
                            CommentID = comment.ID,
                            UserID = blogComment.BlogPost.UserID,
                            ActorID = user.ID,
                            IsNew = true,
                        };
                        repository.CreateFeed(feed);
                    }
                    Console.WriteLine(string.Format("{0} написал комментарий в блоге {1}", user.FirstName, comment.Text.Teaser(50)));

                    if (rand.Next(100) % 2 == 0)
                    {
                        var user2 = users.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                        var replyComment = new Comment()
                        {
                            ParentID = comment.ID,
                            UserID = user2.ID,
                            Text = Textarium.GetRandomText(1 + rand.Next(2)),
                        };
                        repository.CreateComment(replyComment);
                        var replyCommentBlog = new CommentBlogPost()
                        {
                            CommentID = replyComment.ID,
                            BlogPostID = blogPost.ID
                        };
                        repository.CreateCommentBlogPost(replyCommentBlog);
                        if (replyCommentBlog.BlogPost.UserID != user.ID)
                        {
                            var feed = new Feed()
                            {
                                ActionType = (int)Feed.ActionTypeEnum.AddBlogPostComment,
                                BlogPostID = replyCommentBlog.BlogPostID,
                                CommentID = comment.ID,
                                UserID = replyCommentBlog.BlogPost.UserID,
                                ActorID = user.ID,
                                IsNew = true,
                            };
                            repository.CreateFeed(feed);
                        }
                        Console.WriteLine(string.Format("{0} ответил комментарием в блоге {1}", user2.FirstName, replyComment.Text.Teaser(50)));
                    }
                }
            }
        }


        private void GenerateMeetings(int userCount, int meetingCount)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.ToList().OrderBy(p => Guid.NewGuid()).Take(userCount);

            var cities = repository.Cities.ToList();
            foreach (var user in users)
            {
                for (int i = 0; i < meetingCount; i++)
                {
                    if (user.Money >= 50)
                    {

                        var meeting = new Meeting()
                        {
                            UserID = user.ID,
                            MeetingDate = DateTime.Now.AddDays(rand.Next(30)),
                            Text = Textarium.GetRandomText(1).Teaser(140),
                            CityID = cities.OrderBy(p => Guid.NewGuid()).First().ID,
                        };

                        var moneyDetail = new MoneyDetail()
                        {
                            Type = (int)MoneyDetail.TypeEnum.PayForMeeting,
                            Sum = -50,
                            Description = "Оплата создания встречи",
                            UserID = user.ID
                        };
                        var guid = Guid.NewGuid();
                        repository.CreateMoneyDetail(moneyDetail, guid);
                        repository.CreateMeeting(meeting);
                        repository.SubmitMoney(guid);
                    }
                }
            }
        }

        private void GenerateEvents(int userCount, int userEventCount)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.ToList().OrderBy(p => Guid.NewGuid()).Take(userCount);
            var allUsers = repository.Users.ToList();
            var cities = repository.Cities.ToList();
            foreach (var user in users)
            {
                var @event = new Event()
                {
                    UserID = user.ID,
                    EventDate = DateTime.Now.AddDays(rand.Next(30)),
                    Name = Team.GetRandom(),
                    ImagePath = Imaginarium.SaveRandomImage("/Content/files/uploads/"),
                    Place = Team.GetRandom(),
                    Description = Textarium.GetRandomText(1).Teaser(140),
                    CityID = cities.OrderBy(p => Guid.NewGuid()).First().ID,
                    Coordinate = (rand.NextDouble() * 80).ToString() + "|" + (rand.NextDouble() * 80).ToString()
                };
                repository.CreateEvent(@event);
                var userEventCout = rand.Next(userEventCount) + 3;
                for (int i = 0; i < userEventCout; i++)
                {
                    var userE = allUsers.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                    var userEvent = new UserEvent()
                    {
                        EventID = @event.ID,
                        UserID = userE.ID
                    };
                    repository.CreateUserEvent(userEvent);
                }
            }
        }


        public void GenerateEventComments()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.ToList();

            var events = repository.Events.ToList();

            foreach (var @event in events)
            {
                var countNew = rand.Next(3) + 2;
                for (int i = 0; i < countNew; i++)
                {
                    var user = users.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                    var comment = new Comment()
                    {
                        UserID = user.ID,
                        Text = Textarium.GetRandomText(1 + rand.Next(2)),
                    };
                    repository.CreateComment(comment);
                    var eventComment = new CommentEvent()
                    {
                        CommentID = comment.ID,
                        EventID = @event.ID
                    };
                    repository.CreateCommentEvent(eventComment);
                    if (eventComment.Event.UserID != user.ID)
                    {
                        var feed = new Feed()
                        {
                            ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                            EventID = eventComment.EventID,
                            CommentID = comment.ID,
                            UserID = eventComment.Event.UserID,
                            ActorID = user.ID,
                            IsNew = true,
                        };
                        repository.CreateFeed(feed);
                    }
                    Console.WriteLine(string.Format("{0} написал комментарий в блоге {1}", user.FirstName, comment.Text.Teaser(50)));

                    if (rand.Next(100) % 2 == 0)
                    {
                        var user2 = users.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                        var replyComment = new Comment()
                        {
                            ParentID = comment.ID,
                            UserID = user2.ID,
                            Text = Textarium.GetRandomText(1 + rand.Next(2)),
                        };
                        repository.CreateComment(replyComment);
                        var replyEventComment = new CommentEvent()
                        {
                            CommentID = replyComment.ID,
                            EventID = @event.ID
                        };
                        repository.CreateCommentEvent(replyEventComment);
                        if (replyEventComment.Event.UserID != user.ID)
                        {
                            var feed = new Feed()
                            {
                                ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                                EventID = replyEventComment.EventID,
                                CommentID = comment.ID,
                                UserID = replyEventComment.Event.UserID,
                                ActorID = user.ID,
                                IsNew = true,
                            };
                            repository.CreateFeed(feed);
                        }
                        Console.WriteLine(string.Format("{0} ответил комментарием в блоге {1}", user2.FirstName, replyComment.Text.Teaser(50)));
                    }
                }
            }
        }


        private void GenerateGroups(int userCount, int userGroupCount)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var users = repository.Users.ToList().OrderBy(p => Guid.NewGuid()).Take(userCount);

            var cities = repository.Cities.ToList();
            foreach (var user in users)
            {
                for (int i = 0; i < userGroupCount; i++)
                {
                    var group = new Group()
                    {
                        UserID = user.ID,

                        Name = Team.GetRandom(),
                        AvatarUrl = Imaginarium.SaveRandomImage("/Content/files/uploads/", 600, 60),
                        Info = Textarium.GetRandomText(2),
                        IsBanned = false,
                        IsVip = rand.Next(100) % 2 == 0
                    };
                    repository.CreateGroup(group);
                }
            }
        }

        private void GenerateUserGroups()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var allUsers = repository.Users.ToList();
            foreach (var user in allUsers)
            {

                var groups = repository.Groups.ToList().OrderByDescending(p => Guid.NewGuid()).Take(50);

                foreach (var group in groups)
                {
                    var userGroup = new UserGroup()
                    {
                        UserID = user.ID,
                        GroupID = group.ID
                    };

                    repository.CreateUserGroup(userGroup);
                }
            }
        }

        public void GenerateGroupBlogPosts(int countGroup, int countPost, int countComment)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var groups = repository.Groups.ToList().OrderBy(p => Guid.NewGuid()).Take(countGroup);
            var users = repository.Users.ToList();
            foreach (var group in groups)
            {
                var countNew = countPost + rand.Next(4) - 2;
                for (int i = 0; i < countNew; i++)
                {
                    var randomGroupUser = group.SubUsers.ToList().OrderBy(p => Guid.NewGuid()).First();
                    var groupBlogPost = new GroupBlogPost()
                    {
                        GroupID = group.ID,
                        UserID = randomGroupUser.ID,
                        Text = Textarium.GetRandomHtmlText(1 + rand.Next(3)),
                        Attach = rand.Next(12) % 4 == 0 ? Imaginarium.SaveRandomImage("/Content/files/uploads/") : null,
                        Header = Team.GetRandom() + " " + Team.GetRandom(),
                    };
                    repository.CreateGroupBlogPost(groupBlogPost);
                    Console.WriteLine(string.Format("{0} написал в блоге группы {1}", randomGroupUser.FirstName, groupBlogPost.Header));

                    var newestCount = countComment + rand.Next(3) - 2;
                    for (int j = 0; j < newestCount; j++)
                    {
                        var user = users.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                        var comment = new Comment()
                        {
                            UserID = user.ID,
                            Text = Textarium.GetRandomText(1 + rand.Next(2)),
                        };
                        repository.CreateComment(comment);
                        var commentGroupBlogPost = new CommentGroupBlogPost()
                        {
                            CommentID = comment.ID,
                            GroupBlogPostID = groupBlogPost.ID
                        };
                        repository.CreateCommentGroupBlogPost(commentGroupBlogPost);
                        if (commentGroupBlogPost.GroupBlogPost.UserID != user.ID)
                        {
                            var feed = new Feed()
                            {
                                ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                                GroupBlogPostID = commentGroupBlogPost.GroupBlogPost.UserID,
                                CommentID = comment.ID,
                                UserID = commentGroupBlogPost.GroupBlogPost.UserID,
                                ActorID = user.ID,
                                IsNew = true,
                            };
                            repository.CreateFeed(feed);
                        }
                        Console.WriteLine(string.Format("{0} написал комментарий в блоге {1}", user.FirstName, comment.Text.Teaser(50)));
                        if (rand.Next(100) % 2 == 0)
                        {
                            var user2 = users.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                            var replyComment = new Comment()
                            {
                                ParentID = comment.ID,
                                UserID = user2.ID,
                                Text = Textarium.GetRandomText(1 + rand.Next(2)),
                            };
                            repository.CreateComment(replyComment);

                            var replyCommentGroupBlog = new CommentGroupBlogPost()
                            {
                                CommentID = replyComment.ID,
                                GroupBlogPostID = groupBlogPost.ID
                            };
                            repository.CreateCommentGroupBlogPost(replyCommentGroupBlog);
                            if (replyCommentGroupBlog.GroupBlogPost.UserID != user.ID)
                            {
                                var feed = new Feed()
                                {
                                    ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                                    GroupBlogPostID = replyCommentGroupBlog.GroupBlogPost.UserID,
                                    CommentID = comment.ID,
                                    UserID = replyCommentGroupBlog.GroupBlogPost.UserID,
                                    ActorID = user.ID,
                                    IsNew = true,
                                };
                                repository.CreateFeed(feed);
                            }
                            Console.WriteLine(string.Format("{0} ответил комментарием в блоге {1}", user2.FirstName, replyComment.Text.Teaser(50)));
                        }
                    }
                }
            }
        }

        public void GenerateFeeds()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            var friendships = repository.Friendships.Where(p => p.Approved);
            foreach (var friendship in friendships)
            {
                var feed = new Feed()
                {
                    ActionType = (int)Feed.ActionTypeEnum.AddFriend,
                    UserID = friendship.SenderID,
                    ActorID = friendship.ReceiverID,
                    IsNew = true,
                };
                repository.CreateFeed(feed);
            }
            var blogComments = repository.CommentBlogPosts;
            foreach (var blogComment in blogComments)
            {
                if (blogComment.BlogPost.UserID != blogComment.Comment.User.ID)
                {
                    var feed = new Feed()
                    {
                        ActionType = (int)Feed.ActionTypeEnum.AddBlogPostComment,
                        BlogPostID = blogComment.BlogPostID,
                        CommentID = blogComment.Comment.ID,
                        UserID = blogComment.BlogPost.UserID,
                        ActorID = blogComment.Comment.User.ID,
                        IsNew = true,
                    };
                    repository.CreateFeed(feed);
                }
            }

            var groupBlogComments = repository.CommentGroupBlogPosts;
            foreach (var groupBlogComment in groupBlogComments)
            {
                if (groupBlogComment.GroupBlogPost.UserID != groupBlogComment.Comment.User.ID)
                {
                    var feed = new Feed()
                    {
                        ActionType = (int)Feed.ActionTypeEnum.AddGroupBlogPostComment,
                        GroupBlogPostID = groupBlogComment.GroupBlogPostID,
                        CommentID = groupBlogComment.Comment.ID,
                        UserID = groupBlogComment.GroupBlogPost.UserID,
                        ActorID = groupBlogComment.Comment.User.ID,
                        IsNew = true,
                    };
                    repository.CreateFeed(feed);
                }
            }

            var eventComments = repository.CommentEvents;
            foreach (var eventComment in eventComments)
            {
                if (eventComment.Event.UserID != eventComment.Comment.User.ID)
                {
                    var feed = new Feed()
                    {
                        ActionType = (int)Feed.ActionTypeEnum.AddEventComment,
                        EventID = eventComment.EventID,
                        CommentID = eventComment.Comment.ID,
                        UserID = eventComment.Event.UserID,
                        ActorID = eventComment.Comment.User.ID,
                        IsNew = true,
                    };
                    repository.CreateFeed(feed);
                }
            }
        }
    }
}
