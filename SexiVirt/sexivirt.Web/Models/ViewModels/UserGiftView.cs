using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using sexivirt.Model;
using System.ComponentModel.DataAnnotations;
using sexivirt.Web.Global.Auth;
using sexivirt.Web.Global.Config;
using Tool;
using Config = ImageResizer.Configuration.Config;


namespace sexivirt.Web.Models.ViewModels
{

    [Serializable]
    public class UserDataFromNetwork
    {
        public string network { get; set; }
        public string identity { get; set; }
        public string uid { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string bdate { get; set; }
        public string photo { get; set; }
        public string photo_big { get; set; }
        public string city { get; set; }
        public string profile { get; set; }
        public int verified_email { get; set; }
        public int sex { get; set; }

    }

    public class SocialAuthResult
    {
        public Model.User User { get; set; }
        private static IConfig _config;
        protected static IConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = DependencyResolver.Current.GetService<IConfig>();
                }
                return _config;
            }
        }

        public string Message { get; set; }
        public bool HasResult { get; set; }

        private static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
        }


        public static string GeneratePassword(int len = 6)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            string pattern = "0123456789abcdefghijklmnopqrstuvwxyz";
            string pass = "";
            if (len == 0) len = 6;
            for (int i = 0; i < len; i++)
            {
                pass += pattern.Substring(rnd.Next(0, pattern.Length), 1);
            }
            return pass;
        }

    
        public static SocialAuthResult CheckAuth()
        {
/*
            var from = HttpContext.Current.Request["from"];
            if (from.IsNullOrWhiteSpace())
                return new SocialAuthResult();
*/

            var target = String.Format("http://ulogin.ru/token.php?token={0}&host={1}", HttpContext.Current.Request["token"],
                                       HttpContext.Current.Request.Url.Host);

            var wc = new WebClient();
            byte[] data = null;
            try
            {
                data = wc.DownloadData(target);
            }
            catch (Exception exxxx)
            {
                return new SocialAuthResult()
                {
                    HasResult = true,
                    Message = "Ошибка при установлении соединения с сервером авторизации",
                };
            }
            var js = Encoding.UTF8.GetString(data);
            js = DecodeEncodedNonAsciiCharacters(js);
            var serializer = new JavaScriptSerializer();
            var jsData = serializer.Deserialize<UserDataFromNetwork>(js);

            if (string.IsNullOrEmpty(jsData.email))
            {
                return new SocialAuthResult()
                {
                    HasResult = true,
                    Message = "Для регистрации через соцсеть, в соцсети должен быть указан email",
                };
            }

            Model.User user = null;
            try
            {

                var db = new sexivirtDbDataContext(Config.ConnectionStrings("ConnectionString"));
                user = db.Users.FirstOrDefault(x => x.Email.ToLower() == jsData.email.ToLower());


                //нет такого
                if (user == null)
                {
                    var pass = GeneratePassword(6);
                    DateTime bd = DateTime.MinValue;
                    DateTime.TryParse(jsData.bdate, out bd);
                    //SiteExceptionLog.WriteToLog("Creating user = "+jsData.email);
                    user = new Model.User()
                    {
                        ActivatedDate = DateTime.Now,
                        AddedDate = DateTime.Now,
                        LastVisitDate = DateTime.Now,
                        ActivatedLink =  StringExtension.GenerateNewFile(),
                        Email = jsData.email,
                        Password = pass,
                        Sex = jsData.sex == 2,
                        
                        Login = jsData.email,
                        FirstName = (jsData.first_name ?? "") + (jsData.last_name ?? ""),
                        

                    };

                    if (bd != DateTime.MinValue)
                    {
                        user.Birthday = bd;
                    }
    
                    
                    var al = jsData.photo_big.IsNullOrWhiteSpace() ? jsData.photo : jsData.photo_big;
                    if (!al.IsNullOrWhiteSpace())
                    {
                        string ap = "/Content/files/avatars/" + Guid.NewGuid() +
                                    al.Substring(al.Length - 4);

                        try
                        {

                            wc.DownloadFile(al, HttpContext.Current.Server.MapPath(ap));
                            user.AvatarPath = ap;
                        }
                        catch
                        {
                            
                        }
                    }
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();
                    
                }
                //есть чувак
                else
                {
                    //мыло подтверждено и совпало, логин совпал
                    if ((/*jsData.verified_email == 1 && */jsData.email.ToLower() == user.Email.ToLower()))
                    {

                    }
                    //редирект на страницу с формой, где выводим сообщение
                    else
                    {
                        return new SocialAuthResult()
                        {
                            HasResult = false,
                            Message = (jsData.nickname == user.FirstName
                                              ? "Пользователь с таким логином уже зарегистрирован. Пожалуйста, укажите другой логин."
                                              : "Пользователь с таким Email уже зарегистрирован. Пожалуйста укажите другой Email"),
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new SocialAuthResult()
                {
                    HasResult = false,
                    Message = ex.Message,
                };

            }
            return new SocialAuthResult()
            {
                User = user,
                HasResult = true,
                Message = "",
            };

        }
    }


	public class UserGiftView
    {
        public int ID { get; set; }

        public int? GiftType { get; set; }

        [Required(ErrorMessage="Выберите подарок")]
		public int? GiftID {get; set; }

		public int SenderID {get; set; }

		public int ReceiverID {get; set; }

		public string Text {get; set; }

		public bool Visible {get; set; }

    }
}