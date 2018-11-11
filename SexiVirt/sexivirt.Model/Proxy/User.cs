using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sexivirt.Model
{
    public partial class User
    {
        public EntitySet<UserRating> ReceiveUserRatings
        {
            get
            {
                return UserRatings;
            }
        }

        public EntitySet<UserRating> SendUserRatings
        {
            get
            {
                return UserRatings1;
            }
        }

        public EntitySet<UserGift> ReceivedUserGifts
        {
            get
            {
                return UserGifts;
            }
        }

        public EntitySet<UserGift> SendedUserGifts
        {
            get
            {
                return UserGifts1;
            }
        }

        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                var hasRole = UserRoles.Any(p => string.Compare(p.Role.Code, role, true) == 0);
                if (hasRole)
                {
                    return true;
                }
            }
            return false;
        }

        private User _admin;
        public User Admin
        {
            get
            {
                return _admin ?? (_admin =
                    new sexivirtDbDataContext(
                        ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString).Users
                        .FirstOrDefault(x => x.Login == "admin"));
            }
        }


        public bool IsOnline
        {
            get
            {
 /*               if (LastVisitDate.AddMinutes(5) <= DateTime.Now)
                {
                    if (AdminChats.Any(x => x.UserID == ID))
                    {
                        var admin = Admin;

                        if (admin == null)
                            return false;

                        if (admin.ID == ID)
                            return true;
                        else if (admin.IsOnline)
                        {
                            return true;
                        }
                        else
                        {
                            var rnd = new Random(DateTime.Now.Millisecond).Next(1, 100);
                            if (DateTime.Now.Hour > 10 && DateTime.Now.Hour < 20)
                            {
                                return rnd > 30;
                            }
                            else
                            {
                                return rnd > 70;
                            }
                        }
                    }
                }*/
                return LastVisitDate.AddMinutes(5) > DateTime.Now;
            }
        }

        public string OnlineStr
        {
            get
            {
                return IsOnline ? "active" : "";
            }
        }

        public bool IsActivated
        {
            get { return ActivatedDate.HasValue; }
        }

        public string FullAvatarPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AvatarPath))
                {
                    if (Sex)
                    {
                        return "/Content/images/man-avatar.png";
                    }
                    return "/Content/images/woman-avatar.png";

                }
                return AvatarPath;
            }
        }


        public int CalculatedRating
        {
            get { return UserRatings.Sum(x => x.Mark); }
        }

        public int Age
        {
            get
            {
                if (Birthday.HasValue)
                {
                    int age = DateTime.Now.Year - Birthday.Value.Year;
                    if (Birthday.Value > DateTime.Now.AddYears(-age))
                    {
                        age--;
                    }
                    return age;
                }
                return 0;

            }
        }

        public IList<Preference> SubPreferences
        {
            get
            {
                return UserPreferences.Select(p => p.Preference).ToList();
            }
        }

        public IEnumerable<UserGift> SubGifts
        {
            get
            {
                return ReceivedUserGifts.OrderByDescending(p => p.AddedDate).AsEnumerable();
            }
        }

        public IEnumerable<Album> SubAlbums
        {
            get
            {
                return Albums.OrderBy(p => p.OrderBy).AsEnumerable();
            }
        }
        public IEnumerable<Album> SubAlbumsForHome
        {
            get
            {
                var albums = Albums.Where(x => x.Price.HasValue).OrderByDescending(p => p.Price).Take(2).ToList();
                var list =
                    albums.Concat(Albums.Where(x => !x.Price.HasValue).OrderBy(x => x.OrderBy).Take(5));
                return list.Distinct().Take(3);
            }
        }

        public bool HasAccess(Album album)
        {
            if (album.Price > 0)
            {
                return AlbumAccesses.Any(p => p.AlbumID == album.ID);
            }
            if (album.UserID == ID)
            {
                return true;
            }
            return true;
        }

        public IEnumerable<BlogPost> SubBlogPosts
        {
            get
            {
                return BlogPosts.OrderByDescending(p => p.AddedDate).AsEnumerable();
            }
        }

        public IEnumerable<Event> SubEvents
        {
            get
            {
                return Events.OrderByDescending(p => p.EventDate).AsEnumerable();
            }
        }

        public string GenderStr
        {
            get
            {
                return Sex ? "man" : "woman";
            }
        }

        public bool HasPreference(int idPreference)
        {
            return UserPreferences.Any(p => p.PreferenceID == idPreference);
        }

        public EntitySet<Message> IncomeMessages
        {
            get
            {
                return Messages;
            }
            set
            {
                Messages = value;
            }
        }

        public EntitySet<Message> OutgoingMessages
        {
            get
            {
                return Messages1;
            }
            set
            {
                Messages1 = value;
            }
        }

        public int UnreadMessagesCount
        {
            get
            {
                return IncomeMessages.Count(p => !p.ReadedDate.HasValue && !p.IsSend);
            }
        }

        public IEnumerable<Photo> SubPhotos
        {
            get
            {
                return Albums.SelectMany(p => p.Photos).OrderBy(p => p.ID);
            }
        }

        public IQueryable<Photo> Photos
        {
            get
            {
                return Albums.SelectMany(p => p.Photos).AsQueryable();
            }
        }

        public int PhotosCount
        {
            get
            {
                return Albums.SelectMany(p => p.Photos).Count();
            }
        }

        public bool IsVotedFor(int userID, bool positive)
        {
            var entry = SendUserRatings.FirstOrDefault(p => p.ReceiverID == userID);
            if (entry != null)
            {
                if ((entry.Mark == 1 && positive) || (entry.Mark == -1 && !positive))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsVotedForComment(int commentID, bool positive)
        {
            var entry = CommentRatings.FirstOrDefault(p => p.CommentID == commentID);
            if (entry != null)
            {
                if ((entry.Mark == 1 && positive) || (entry.Mark == -1 && !positive))
                {
                    return true;
                }
            }
            return false;
        }

        public EntitySet<Blocked> ActiveBlocks
        {
            get
            {
                return Blockeds1;
            }
        }

        public EntitySet<Blocked> PassiveBlocks
        {
            get
            {
                return Blockeds;
            }
        }

        public EntitySet<Friendship> ActiveFriendships
        {
            get
            {
                return Friendships1;
            }
        }
        public EntitySet<Friendship> PassiveFriendships
        {
            get
            {
                return Friendships;
            }
        }



        public List<Friendship> FullFriendships
        {
            get
            {
                return ActiveFriendships.Where(p => p.Approved).ToList();
            }
        }

        public List<Friendship> MeAskedFriendships
        {
            get
            {
                return PassiveFriendships.OrderByDescending(p => p.ID).Where(p => !p.Approved).ToList();
            }
        }


        public bool HasFriend(int id)
        {
            return FullFriendships.Any(p => p.ReceiverID == id);
        }

        public bool AskForFriend(int id)
        {
            return ActiveFriendships.Any(p => p.ReceiverID == id);
        }

        public bool MeAskedForFriendship(int id)
        {
            return PassiveFriendships.Any(p => p.SenderID == id);
        }

        public Friendship MeAskedFriendship(int id)
        {
            return PassiveFriendships.FirstOrDefault(p => p.SenderID == id);
        }

        public bool AskedForNewFriend
        {
            get
            {
                return PassiveFriendships.Any(p => !p.Approved);
            }
        }

        public int CountAskedForNewFriend
        {
            get
            {
                return PassiveFriendships.Count(p => !p.Approved);
            }
        }

        public bool CanAskForFriend(int id)
        {
            return !ActiveFriendships.Any(p => p.ReceiverID == id);
        }

        public bool BlockedBy(int id)
        {
            return PassiveBlocks.Any(p => p.SenderID == id);
        }

        public int BlogPostCount
        {
            get
            {
                return BlogPosts.Count;
            }
        }

        public IEnumerable<Group> MyGroups
        {
            get
            {

                return Groups.Concat(UserGroups.Select(x => x.Group)).Distinct().OrderByDescending(p => p.Rating).AsEnumerable();
            }
        }

        public bool InEvent(Event @event)
        {
            return UserEvents.Any(p => p.EventID == @event.ID);
        }
        public bool InGroup(Group group)
        {
            return UserGroups.Any(p => p.GroupID == group.ID);
        }

        public bool HasConversation(int id)
        {
            return Connects.Any(x => x.OtherUserID == id || x.UserID == id);
        }
    }
}
