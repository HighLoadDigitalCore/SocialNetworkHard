using sexivirt.Model;
using sexivirt.Web.Models.ViewModels.Info;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Tool;

namespace sexivirt.Web.Global
{
    public class SearchEngine
    {
        #region Get
        public static IEnumerable<City> Get(string SearchString, IQueryable<City> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank;
            foreach (var entry in source)
            {
                rank = 0;
                if (entry.Name != null)
                {
                    rank += Regex.Matches(entry.Name.ToLowerInvariant(), regex).Count;
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }


        #endregion

        internal static IEnumerable<Friendship> Search(string searchString, IEnumerable<Friendship> source)
        {
            var term = StringExtension.CleanContent(searchString.ToLowerInvariant().Trim(), false).Trim();
            foreach (var entry in source)
            {
                if (entry.Receiver.FirstName != null && (entry.Receiver.FirstName.ToLowerInvariant().StartsWith(term)
                    || entry.Receiver.FirstName.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    yield return entry;
                    continue;
                }
                if (entry.Receiver.Email != null && (entry.Receiver.Email.ToLowerInvariant().StartsWith(term)
                   || entry.Receiver.Email.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    yield return entry;
                    continue;
                }
            }
        }

        internal static IEnumerable<Friendship> SearchNewContacts(string searchString, IEnumerable<Friendship> source)
        {
            var term = StringExtension.CleanContent(searchString.ToLowerInvariant().Trim(), false).Trim();

            foreach (var entry in source)
            {
                if (entry.Sender.FirstName != null && (entry.Sender.FirstName.ToLowerInvariant().StartsWith(term)
                    || entry.Sender.FirstName.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    yield return entry;
                    continue;
                }
                if (entry.Sender.Email != null && (entry.Sender.Email.ToLowerInvariant().StartsWith(term)
                   || entry.Sender.Email.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    yield return entry;
                    continue;
                }
            }
        }

        internal static IEnumerable<Model.Group> Search(string searchString, IEnumerable<Model.Group> source)
        {
            var term = StringExtension.CleanContent(searchString.ToLowerInvariant().Trim(), false).Trim();
            foreach (var entry in source)
            {
                if (entry.Name != null && (entry.Name.ToLowerInvariant().StartsWith(term)
                   || entry.Name.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    yield return entry;
                    continue;
                }
            }
        }   
        internal static IEnumerable<Model.Group> SearchAll(string searchString, IEnumerable<Model.Group> source)
        {
            var term = StringExtension.CleanContentKeepShortWords(searchString.ToLowerInvariant().Trim(), false).Trim();
            foreach (var entry in source)
            {
                if (entry.Name != null && (entry.Name.ToLowerInvariant().StartsWith(term)
                   || entry.Name.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    yield return entry;
                    continue;
                }
            }
        }
    }
}