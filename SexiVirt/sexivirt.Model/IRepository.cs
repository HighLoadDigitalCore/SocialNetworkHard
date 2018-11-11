using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sexivirt.Model
{
    public interface IRepository
    {
        IQueryable<T> GetTable<T>() where T : class;

        #region Role

        IQueryable<Role> Roles { get; }

        bool CreateRole(Role instance);

        bool RemoveRole(int idRole);

        #endregion

        #region User

        IQueryable<AdminChat> ChatUsers { get; }
        IQueryable<User> Users { get; }
        IQueryable<Setting> Settings { get; }
        bool UpdateSetting(string name, string value);
        Setting GetSetting(string name);

        bool CreateUser(User instance);
        bool CreateChatUser(AdminChat instance);

        bool UpdateUser(User instance);
        bool UpdateUserVisitDate(User instance);

        User GetUser(string email);

        User Login(string login, string password);

        bool ActivateUser(User instance);
        User ActivateUser(string link);

        bool ChangePassword(User instance);

        bool ChangeMail(User instance);

        void VisitUser(int id);

        #endregion

        #region UserRole

        IQueryable<UserRole> UserRoles { get; }

        bool CreateUserRole(UserRole instance);

        bool RemoveUserRole(int idUserRole);

        #endregion

        #region Album
        
        IQueryable<Album> Albums { get; }
        
        bool CreateAlbum(Album instance);
        bool DeleteAlbum(int id);
        
        bool UpdateAlbum(Album instance);
        
        bool RemoveAlbum(int idAlbum);
        
        #endregion 

        #region AlbumAccess
        
        IQueryable<AlbumAccess> AlbumAccesses { get; }
        
        bool CreateAlbumAccess(AlbumAccess instance);
        
        bool UpdateAlbumAccess(AlbumAccess instance);
        
        bool RemoveAlbumAccess(int idAlbumAccess);
        
        #endregion 

        #region Blocked
        
        IQueryable<Blocked> Blockeds { get; }
        
        bool BlockUser(int senderId, int receverId);

        bool UnblockUser(int senderId, int receverId);
        
        #endregion 

        #region BlockGroupUser
        
        IQueryable<BlockGroupUser> BlockGroupUsers { get; }
        
        bool CreateBlockGroupUser(BlockGroupUser instance);
        
        bool UpdateBlockGroupUser(BlockGroupUser instance);
        
        bool RemoveBlockGroupUser(int idBlockGroupUser);
        
        #endregion 

        #region BlogPost
        
        IQueryable<BlogPost> BlogPosts { get; }
        
        bool CreateBlogPost(BlogPost instance);
        
        bool UpdateBlogPost(BlogPost instance);
        
        bool RemoveBlogPost(int idBlogPost);
        
        #endregion 

        #region City
        
        IQueryable<City> Cities { get; }
        
        bool CreateCity(City instance);
        
        bool UpdateCity(City instance);
        
        bool RemoveCity(int idCity);
        
        #endregion 

        #region ChangePassword

        IQueryable<ChangePassword> ChangePasswords { get; }

        bool CreateChangePassword(ChangePassword instance);

        bool UpdateChangePassword(ChangePassword instance);

        bool RemoveChangePassword(int idChangePassword);

        #endregion 

        #region Comment
        
        IQueryable<Comment> Comments { get; }
        
        bool CreateComment(Comment instance);
        
        bool UpdateComment(Comment instance);
        
        bool RemoveComment(int idComment, int? userID = null);
        
        #endregion 

        #region CommentEvent
        
        IQueryable<CommentEvent> CommentEvents { get; }
        
        bool CreateCommentEvent(CommentEvent instance);
        
        bool UpdateCommentEvent(CommentEvent instance);
        
        bool RemoveCommentEvent(int idCommentEvent, int? userID);
        
        #endregion 

        #region CommentBlogPost

        IQueryable<CommentBlogPost> CommentBlogPosts { get; }

        bool CreateCommentBlogPost(CommentBlogPost instance);

        bool UpdateCommentBlogPost(CommentBlogPost instance);

        bool RemoveCommentBlogPost(int idCommentBlogPost, int? userID = null);

        #endregion 

        #region Connect
        
        IQueryable<Connect> Connects { get; }
        
        bool CreateConnect(Connect instance);
        
        bool UpdateConnect(Connect instance);
        
        bool RemoveConnect(int idConnect);
        
        #endregion 

        #region DepositCandidate
        
        IQueryable<DepositCandidate> DepositCandidates { get; }
        
        bool CreateDepositCandidate(DepositCandidate instance);
        
        bool UpdateDepositCandidate(DepositCandidate instance);
        
        bool RemoveDepositCandidate(int idDepositCandidate);
        
        #endregion 

        #region Event
        
        IQueryable<Event> Events { get; }
        
        bool CreateEvent(Event instance);
        
        bool UpdateEvent(Event instance);
        
        bool RemoveEvent(int idEvent);
        
        #endregion 

        #region Feed
        
        IQueryable<Feed> Feeds { get; }
        
        bool CreateFeed(Feed instance);
        
        bool RemoveFeed(int idFeed);

        bool ReadFeed(Feed feed);
        
        #endregion 

        #region Friendship
        
        IQueryable<Friendship> Friendships { get; }
        
        bool CreateFriendship(Friendship instance);

        bool ConfirmFriendship(int idFriendship);

        bool DeclineFriendship(int idFriendship);

        bool RemoveFriend(int idUser, int idFriend);
        
        #endregion 

        #region Gift
        
        IQueryable<Gift> Gifts { get; }
        
        bool CreateGift(Gift instance);
        
        bool UpdateGift(Gift instance);
        
        bool RemoveGift(int idGift);
        
        #endregion 

        #region Group
        
        IQueryable<Group> Groups { get; }
        
        bool CreateGroup(Group instance);
        
        bool UpdateGroup(Group instance);
        
        bool RemoveGroup(int idGroup);
        
        #endregion 

        #region GroupBlogPost
        
        IQueryable<GroupBlogPost> GroupBlogPosts { get; }
        
        bool CreateGroupBlogPost(GroupBlogPost instance);
        
        bool UpdateGroupBlogPost(GroupBlogPost instance);
        
        bool RemoveGroupBlogPost(int idGroupBlogPost);
        
        #endregion 

        #region Meeting
        
        IQueryable<Meeting> Meetings { get; }
        
        bool CreateMeeting(Meeting instance);
        
        bool UpdateMeeting(Meeting instance);
        
        bool RemoveMeeting(int idMeeting);
        
        #endregion 

        #region Message
        
        IQueryable<Message> Messages { get; }
        
        bool CreateMessage(Message instance);
        
        
        bool RemoveMessage(int idMessage);

        bool ReadMessages(int idReader, int idConnect);

        bool RemoveAllMessages(int idReader, int idConnect);
        
        #endregion 

        #region MoneyDetail
        
        IQueryable<MoneyDetail> MoneyDetails { get; }

        Guid CreateTripleMoneyDetail(MoneyDetail from, MoneyDetail to, MoneyDetail fee = null);

        bool CreateMoneyDetail(MoneyDetail instance, Guid uniqueGuid);

        void RecalculateUserMoney(int userID);
        bool SubmitMoney(Guid guid);

        bool DiscardMoney(Guid guid);

        void RecalculateAll();

        void RemoveUnsubmitted();

        void RemoveMoneyTransaction(Guid guid);

        #endregion 

        #region MoneyWithdraw
        
        IQueryable<MoneyWithdraw> MoneyWithdraws { get; }
        
        bool CreateMoneyWithdraw(MoneyWithdraw instance);
        
        bool UpdateMoneyWithdraw(MoneyWithdraw instance);
        bool UpdateMoneyWithdrawStatus(MoneyWithdraw instance);
        
        bool RemoveMoneyWithdraw(int idMoneyWithdraw);
        
        #endregion 

        #region Photo
        
        IQueryable<Photo> Photos { get; }
        
        bool CreatePhoto(Photo instance);
        
        bool UpdatePhoto(Photo instance);
        
        bool RemovePhoto(int idPhoto);

        bool UpdateAlbumPhotos(List<Photo> photos, int idAlbum);

        Photo ChangePhoto(int idCurrentPhoto, bool next);

        Photo ChangeAllPhoto(int idCurrentPhoto, bool next);
        

        #endregion 

        #region Preference
        
        IQueryable<Preference> Preferences { get; }
        
        bool CreatePreference(Preference instance);
        
        bool UpdatePreference(Preference instance);
        
        bool RemovePreference(int idPreference);
        
        #endregion 
    
        #region UserEventRating
        
        IQueryable<UserEventRating> UserEventRatings { get; }
        
        bool CreateUserEventRating(UserEventRating instance);
        
        bool UpdateUserEventRating(UserEventRating instance);
        
        bool RemoveUserEventRating(int idUserEventRating);
        
        #endregion 

        #region UserGift
        
        IQueryable<UserGift> UserGifts { get; }
        
        bool CreateUserGift(UserGift instance);
        
        bool UpdateUserGift(UserGift instance);
        
        bool RemoveUserGift(int idUserGift);
        
        #endregion 

        #region UserPreference
        
        IQueryable<UserPreference> UserPreferences { get; }
        
        bool CreateUserPreference(UserPreference instance);
        
        bool UpdateUserPreference(UserPreference instance);
        
        bool RemoveUserPreference(int idUserPreference);
        
        #endregion 

        #region UserRating
        
        IQueryable<UserRating> UserRatings { get; }
        
        bool CreateUserRating(UserRating instance);
        
        bool RemoveUserRating(int idUserRating);
        
        #endregion 

        #region UserStatus
        
        IQueryable<UserStatus> UserStatus { get; }
        
        bool CreateUserStatus(UserStatus instance);
        
        bool RemoveUserStatus(int idUserStatus);
        
        #endregion 
      
        #region UserEvent

        IQueryable<UserEvent> UserEvents { get; }

        bool CreateUserEvent(UserEvent instance);

        bool RemoveUserEvent(int idUserEvent);

        #endregion 

        #region CommentRating
        
        IQueryable<CommentRating> CommentRatings { get; }
        
        bool CreateCommentRating(CommentRating instance);
        
        bool RemoveCommentRating(int idCommentRating, int? userID = null);
        
        #endregion 

        #region CommentGroupBlogPost
        
        IQueryable<CommentGroupBlogPost> CommentGroupBlogPosts { get; }
        
        bool CreateCommentGroupBlogPost(CommentGroupBlogPost instance);
        
        bool UpdateCommentGroupBlogPost(CommentGroupBlogPost instance);
        
        bool RemoveCommentGroupBlogPost(int idCommentGroupBlogPost, int? userID = null);
        
        #endregion 

        #region UserGroup
        
        IQueryable<UserGroup> UserGroups { get; }
        IQueryable<MessagePayment> MessagePayments { get; }
        bool UpdatePayment(MessagePayment payment);
        bool CreatePayment(MessagePayment payment);

        bool CreateUserGroup(UserGroup instance);
        
        bool UpdateUserGroup(UserGroup instance);
        
        bool RemoveUserGroup(int idUserGroup);
        
        #endregion

        bool UpdateUserAdmin(User user);
        bool UpdateUserIp(User user);
        bool UpdateUserPayMessageDate(User user, int days);
        int CreateRoboTransaction(decimal sum, int userID);
        RoboTransaction GetRoboTransaction(int id);
        bool SubmitRoboTransaction(RoboTransaction order);
        bool FailRoboTransaction(RoboTransaction order);
        void DeleteMessagePayment(int id);
        void DeleteChatUser(int userid);
    }
}
