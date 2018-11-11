using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tool
{
    public static class StringExtension
    {
        public static T ConvertFromString<T>(this string text)
        {
            if (typeof(Enum).IsAssignableFrom(typeof(T)))
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), text);
                }
                catch
                {
                    return default(T);
                }
            }
            return (T)Convert.ChangeType(text, typeof(T));
        }

        public static IEnumerable<T> Split<T>(this string target, params string[] args)
        {
            if (target == null) return new List<T>();
            return target.Split(args.Any() ? args : new[] { ";", ".", "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Select(x => x.ConvertFromString<T>());
        }


        /// <summary>
        /// The regex strip html.
        /// </summary>
        private static readonly Regex RegexStripHtml = new Regex("<[^>]*>", RegexOptions.Compiled);


        /// <summary>
        /// Remove all tags from html-text 
        /// </summary>
        /// <param name="source">html-text</param>
        /// <returns></returns>
        public static string StripTags(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            source = source.Replace("<br/>", Environment.NewLine);
            source = source.Replace("<br>", Environment.NewLine);
            char[] chArray = new char[source.Length];
            int index = 0;
            bool flag = false;
            for (int i = 0; i < source.Length; i++)
            {
                char ch = source[i];
                if (ch == '<')
                {
                    flag = true;
                }
                else if (ch == '>')
                {
                    flag = false;
                }
                else if (!flag)
                {
                    chArray[index] = ch;
                    index++;
                }
            }
            return new string(chArray, 0, index);

        }

        /// <summary>
        /// Generate new guid-based name
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewFile()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Check on valid html string (not null StripTags or start with image
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsValidHtmlString(this string source)
        {
            return
                !string.IsNullOrWhiteSpace(source) &&
                ((!string.IsNullOrWhiteSpace(source.StripTags()) ||
                (source.IndexOf("<img ", StringComparison.InvariantCultureIgnoreCase) != -1)));
        }


        /// <summary>
        /// Check if is email correct string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(this string source)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is numeric string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string source)
        {
            Regex regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is numeric string with need length
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str, int length)
        {
            Regex regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Match match = regex.Match(str);
            return (match.Success && match.Length == str.Length && str.Length == length);
        }

        /// <summary>
        /// Check if is can be ICQ number (just numbers, -, and space)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsICQ(this string source)
        {
            Regex regex = new Regex(@"[0-9,\-,\x20]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is can be correct phone number
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPhone(this string source)
        {
            Regex regex = new Regex(@"[0-9,\+,\(,\),\-,\x20]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is can be correct url 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(this string source)
        {
            Regex regex = new Regex(@"[a-z,A-Z,\-,_,0-9,\.,\/,?,=,%,а-я,А-Я,:,;,&]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// \n to br tags 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string NlToBr(this string source)
        {
            if (source == null)
            {
                return string.Empty;
            }
            return source.Replace(Environment.NewLine, "<br />");
        }
        public static string Truncate(this string source, int len = 100)
        {
            if (source == null)
            {
                return string.Empty;
            }
            if (source.Length < len)
                return source;
            var index = source.IndexOf(" ", len);
            if (index < 0)
                return source.Substring(0, len) + "...";
            return source.Substring(0, index) + "...";
        }

        public static string NlToP(this string source)
        {
            if (source == null)
            {
                return string.Empty;
            }
            return string.Format("<p>{0}</p>", source.Replace(Environment.NewLine, "</p><p>"));
        }


        /// <summary>
        /// Create random password
        /// </summary>
        /// <param name="passwordLength">length of password</param>
        /// <returns></returns>
        public static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// To Base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            byte[] byt = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(byt);
        }

        /// <summary>
        /// From base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64(this string str)
        {
            byte[] b = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(b);
        }

        /// <summary>
        /// Clear content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="removeHtml"></param>
        /// <returns></returns>
        public static string CleanContent(string content, bool removeHtml)
        {
            if (removeHtml)
            {
                content = StripHtml(content);
            }

            content =
                content.Replace("\\", string.Empty).Replace("|", string.Empty).Replace("(", string.Empty).Replace(
                    ")", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("*", string.Empty).
                    Replace("?", string.Empty).Replace("}", string.Empty).Replace("{", string.Empty).Replace(
                        "^", string.Empty).Replace("+", string.Empty);

            var words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var word in
                words.Select(t => t.ToLowerInvariant().Trim()).Where(word => word.Length > 1))
            {
                sb.AppendFormat("{0} ", word);
            }

            return sb.ToString();
        }       
        
        public static string CleanContentKeepShortWords(string content, bool removeHtml)
        {
            if (removeHtml)
            {
                content = StripHtml(content);
            }

            content =
                content.Replace("\\", string.Empty).Replace("|", string.Empty).Replace("(", string.Empty).Replace(
                    ")", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("*", string.Empty).
                    Replace("?", string.Empty).Replace("}", string.Empty).Replace("{", string.Empty).Replace(
                        "^", string.Empty).Replace("+", string.Empty);

            var words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var word in
                words.Select(t => t.ToLowerInvariant().Trim()).Where(word => word.Length > 0))
            {
                sb.AppendFormat("{0} ", word);
            }

            return sb.ToString();
        }

        private static string StripHtml(string html)
        {
            return StringIsNullOrWhitespace(html) ? string.Empty : RegexStripHtml.Replace(html, string.Empty).Trim();
        }

        private static bool StringIsNullOrWhitespace(string value)
        {
            return ((value == null) || (value.Trim().Length == 0));
        }


        /// <summary>
        /// Make correct phone numbers to one-view base (use russian locale (8 -> 7))
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string ClearPhone(this string phone)
        {
            var str = "";
            foreach (var @char in phone)
            {
                if (@char >= '0' && @char <= '9')
                {
                    str = str + @char;
                }
            }
            if (str.StartsWith("8"))
            {
                str = "7" + str.Substring(1);
            }
            return str;
        }

        /// <summary>
        /// Make teaser (start string) from content
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="length">need length</param>
        /// <param name="more">more chars</param>
        /// <returns></returns>
        public static string Teaser(this string content, int length, string more = "...")
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            if (content.Length < length)
            {
                return content;
            }

            return content.Substring(0, length) + more;
        }

        public static string CountWord(this int count, string first, string second, string five)
        {
            if (count % 10 == 1 && (int)(count / 10) != 1)
            {
                return first;
            }
            if (count % 10 > 1 && count % 10 < 5 && ((int)(count / 10) % 10) != 1)
            {
                return second;
            }
            return five;
        }

        public static string CountWord(this double count, string first, string second, string five)
        {
            var intCount = (int)count;
            return CountWord(intCount, first, second, five);
        }

        public static string EventDateNamed(this DateTime source)
        {
            string date;
            if (source.Date == DateTime.Now.Date)
            {
                date = "Сегодня";
            }
            else if (source.Date == DateTime.Now.Date.AddDays(1))
            {
                date = "Завтра";
            }
            else if (source.Date == DateTime.Now.Date.AddDays(2))
            {
                date = "Послезавтра";
            }
            else
            {
                date = source.ToString("dd MMMM yyyy");
            }
            return date;
        }
    }
}
