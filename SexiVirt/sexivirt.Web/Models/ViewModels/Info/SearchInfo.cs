using sexivirt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Models.ViewModels.Info
{
    public class SearchInfo
    {
        private readonly IRepository _repository = DependencyResolver.Current.GetService<IRepository>();

        public string SearchString { get; set; }

        public IList<Model.User> Users { get; set; }

        public IList<Model.Event> Events { get; set; }

        public IList<Model.Group> Groups { get; set; }

        public IList<Model.BlogPost> BlogPosts { get; set; }


        public void SearchAll(string searchString)
        {
            SearchString = searchString;
            SearchPeople(searchString);
            SearchEvents(searchString);
            SearchGroups(searchString);
            SearchBlogPosts(searchString);
        }

        public void SearchPeople(string searchString)
        {
            var term = searchString.ToLower();
            Users = new List<Model.User>();
            foreach (var entry in _repository.Users)
            {
                if (entry.FirstName != null && (entry.FirstName.ToLowerInvariant().StartsWith(term)
                    || entry.FirstName.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Users.Add(entry);
                    continue;
                }
                if (entry.Email != null && (entry.Email.ToLowerInvariant().StartsWith(term)
                   || entry.Email.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Users.Add(entry);
                    continue;
                }
                if (entry.Status != null && (entry.Status.ToLowerInvariant().StartsWith(term)
                   || entry.Status.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Users.Add(entry);
                    continue;
                }
            }
        }

        public void SearchEvents(string searchString)
        {
            var term = searchString.ToLower();
            Events = new List<Model.Event>();
            foreach (var entry in _repository.Events)
            {
                if (entry.Description != null && (entry.Description.ToLowerInvariant().StartsWith(term)
                    || entry.Description.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Events.Add(entry);
                    continue;
                }
                if (entry.Name != null && (entry.Name.ToLowerInvariant().StartsWith(term)
                   || entry.Name.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Events.Add(entry);
                    continue;
                }
                if (entry.Place != null && (entry.Place.ToLowerInvariant().StartsWith(term)
                   || entry.Place.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Events.Add(entry);
                    continue;
                }
            }
        }

        public void SearchGroups(string searchString)
        {
            var term = searchString.ToLower();
            Groups = new List<Model.Group>();
            foreach (var entry in _repository.Groups)
            {
                if (entry.Info != null && (entry.Info.ToLowerInvariant().StartsWith(term)
                    || entry.Info.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Groups.Add(entry);
                    continue;
                }
                if (entry.Name != null && (entry.Name.ToLowerInvariant().StartsWith(term)
                   || entry.Name.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    Groups.Add(entry);
                    continue;
                }
            }
        }

        public void SearchBlogPosts(string searchString)
        {
            var term = searchString.ToLower();
            BlogPosts = new List<Model.BlogPost>();
            foreach (var entry in _repository.BlogPosts.OrderByDescending(p => p.AddedDate))
            {
                if (entry.Header != null && (entry.Header.ToLowerInvariant().StartsWith(term)
                    || entry.Header.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    BlogPosts.Add(entry);
                    continue;
                }
                if (entry.Text != null && (entry.Text.ToLowerInvariant().StartsWith(term)
                   || entry.Text.ToLowerInvariant().IndexOf(" " + term) != -1))
                {
                    BlogPosts.Add(entry);
                    continue;
                }
            }
        }
    }
}