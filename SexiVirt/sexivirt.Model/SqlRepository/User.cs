using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tool;


namespace sexivirt.Model
{
    public partial class SqlRepository
    {
        public IQueryable<AdminChat> ChatUsers
        {
            get { return Db.AdminChats; }
        }

        public IQueryable<User> Users
        {
            get
            {
                return Db.Users;
            }
        }

        public bool CreateUser(User instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.LastVisitDate = DateTime.Now;
                instance.ActivatedLink = StringExtension.GenerateNewFile();
                Db.Users.InsertOnSubmit(instance);
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        public bool CreateChatUser(AdminChat instance)
        {
            if (instance.ID == 0)
            {
                Db.AdminChats.InsertOnSubmit(instance);
                Db.AdminChats.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        public void DeleteChatUser(int userid)
        {
            var instance = Db.AdminChats.FirstOrDefault(x => x.UserID == userid);
            if (instance != null)
            {
                Db.AdminChats.DeleteOnSubmit(instance);
                Db.AdminChats.Context.SubmitChanges();
            }
        }

        public User GetUser(string login)
        {
            return Db.Users.FirstOrDefault(p => string.Compare(p.Login, login, true) == 0);
        }

        public User Login(string login, string password)
        {
            return Db.Users.FirstOrDefault(p => string.Compare(p.Login, login, true) == 0 && p.Password == password);
        }

        public bool UpdateUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                
                cache.FirstName = instance.FirstName;
                cache.Birthday = instance.Birthday;
                cache.CityID = instance.CityID;
                cache.Height = instance.Height;
                cache.Weight = instance.Weight;
                cache.Description = instance.Description;
                cache.AvatarPath = instance.AvatarPath;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }   
        
        public bool UpdateUserVisitDate(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                try
                {
                    cache.LastVisitDate = instance.LastVisitDate;
                    Db.Users.Context.SubmitChanges();
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool UpdateUserAdmin(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.FirstName = instance.FirstName;
                cache.Email = instance.Email;
                cache.StartIP = instance.StartIP;
                cache.EndIP = instance.EndIP;
                cache.BlockTill = instance.BlockTill;
                cache.BlockType = instance.BlockType;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateUserIp(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.StartIP = instance.StartIP;
                cache.EndIP = instance.EndIP;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateUserPayMessageDate(User instance, int days)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                if (!cache.PayedMessage.HasValue)
                {
                    cache.PayedMessage = DateTime.Now.AddDays(days);
                }
                else if (cache.PayedMessage.Value < DateTime.Now)
                {
                    cache.PayedMessage = DateTime.Now.AddDays(days);
                }
                else if (cache.PayedMessage.Value >= DateTime.Now)
                {
                    cache.PayedMessage = cache.PayedMessage.Value.AddDays(days);
                }
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

      

        public bool ActivateUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ActivatedDate = DateTime.Now;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ChangePassword(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Password = instance.Password;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }
        public bool ChangeMail(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Email = instance.Email;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public void VisitUser(int id)
        {
            try
            {
                var user = Db.Users.FirstOrDefault(p => p.ID == id);
                if (user != null)
                {
                    user.LastVisitDate = DateTime.Now;
                    Db.Users.Context.SubmitChanges();
                }
            }
            catch { }
        }

        public User ActivateUser(string link)
        {
            var user = Db.Users.FirstOrDefault(p => p.ActivatedLink == link);
            if (user != null)
            {
                user.ActivatedDate = DateTime.Now;
                Db.Users.Context.SubmitChanges();
                return user;
            }
            return null;
        }
    }
}
