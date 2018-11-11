using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace sexivirt.Web.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (!(value is string))
            {
                return true;
            }

            var source = value as string;

            var regex = new Regex(@"[0-9,\+,\(,\),\-,\x20]+", RegexOptions.Compiled);
            var match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }
    }
}