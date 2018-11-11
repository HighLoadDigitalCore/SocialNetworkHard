using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using sexivirt.Model;
using Tool;

namespace sexivirt.Web.Global.Auth
{
    public class CustomAuthentication : IAuthentication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string cookieName = "__AUTH_COOKIE";

        public IAuthCookieProvider AuthCookieProvider { get; set; }

        [Inject]
        public IRepository Repository { get; set; }

        #region IAuthentication Members

        public User Login(string userName, string Password, bool isPersistent)
        {
            User retUser = Repository.Login(userName, Password);

            if (retUser != null)
            {

                string addr = HttpContext.Current.Request.UserHostAddress;
                long? intAddress = addr.ToIPInt();

                if (retUser.BlockType == 0)
                {

                    if (retUser.BlockTill.HasValue && retUser.BlockTill.Value > DateTime.Now)
                    {
                        retUser.StartIP = intAddress;
                        retUser.EndIP = intAddress;
                        Repository.UpdateUserIp(retUser);

                        return new User() {FirstName = "Locked"};
                    }
                }
                else if (retUser.BlockType == 1)
                {
                    if (retUser.BlockTill.HasValue && retUser.BlockTill.Value > DateTime.Now)
                    {
                        if (retUser.StartIP.HasValue && retUser.EndIP.HasValue)
                        {
                            if (intAddress.HasValue && retUser.StartIP.Value <= intAddress &&
                                intAddress <= retUser.EndIP.Value)
                            {
                                return new User() {FirstName = "Locked"};
                            }
                        }
                    }
                }
                else
                {
                    retUser.StartIP = intAddress;
                    retUser.EndIP = intAddress;
                    Repository.UpdateUserIp(retUser);
                }
            }

            if (retUser != null)
            {
                CreateCookie(userName, isPersistent);
            }
            return retUser;
        }

        public User Login(string userName)
        {
            User retUser = Repository.Users.FirstOrDefault(p => string.Compare(p.Login, userName, true) == 0);
           
            if (retUser != null)
            {
                CreateCookie(userName);
            }
            return retUser;
        }
      

        public void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var AuthCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            AuthCookieProvider.SetCookie(AuthCookie);
        }

        public void LogOut()
        {
            var httpCookie = new HttpCookie(cookieName)
            {
                Value = string.Empty,
                Expires = DateTime.Now
            };
            AuthCookieProvider.SetCookie(httpCookie);
        }

        private IPrincipal _currentUser;

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = AuthCookieProvider.GetCookie(cookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, Repository);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Failed authentication: " + ex.Message);
                        _currentUser = new UserProvider(null, null);
                    }
                }
                return _currentUser;
            }
        }
        #endregion


    }
}