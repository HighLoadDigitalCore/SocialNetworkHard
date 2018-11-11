using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tool
{
    public static class IPExtensions
    {
        public static string GetRequestIP(this HttpRequestBase request)
        {
            var sIpAddress =
                request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            return String.IsNullOrEmpty(sIpAddress)
                       ? request.ServerVariables["REMOTE_ADDR"]
                       : sIpAddress.Split<string>(", ").ToArray()[0];
        }
        public static string GetRequestIP(this HttpRequest request)
        {
            var sIpAddress =
                request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            return String.IsNullOrEmpty(sIpAddress)
                       ? request.ServerVariables["REMOTE_ADDR"]
                       : sIpAddress.Split<string>(", ").ToArray()[0];
        }

        public static long? ToIPInt(this string ip)
        {
            Int64? ipInteger = null;
            try
            {
                ipInteger = (long)(uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0));
            }
            catch (Exception)
            { }

            return ipInteger;

        }
        public static string ToIPString(this long? value)
        {
            if (!value.HasValue || value.Value < 0)
                return "";
            else if (value.Value < 4294967295)
                return IPAddress.Parse(value.Value.ToString()).ToString();
            else
                return "255.255.255.255";
        }


    }
}
