﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Models.ViewModels.User
{
    public class UserInfoView
    {

        public DateTime Birthdate
        {
            get
            {
                return new DateTime(BirthdateYear, BirthdateMonth, BirthdateDay);
            }

            set
            {
                BirthdateDay = value.Day;
                BirthdateMonth = value.Month;
                BirthdateYear = value.Year;
            }
        }


        public int Age
        {
            get
            {
                int age = DateTime.Now.Year - Birthdate.Year;
                if (Birthdate > DateTime.Now.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        public int BirthdateDay { get; set; }

        public int BirthdateMonth { get; set; }

        public int BirthdateYear { get; set; }

        public IEnumerable<SelectListItem> BirthdateDaySelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdateDay == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> BirthdateMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = BirthdateMonth == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> BirthdateYearSelectList
        {
            get
            {
                for (int i = DateTime.Now.Year - 50; i <= DateTime.Now.Year - 18; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdateYear == i
                    };
                }
            }
        }

        [Display(Name="Рост")]
        public int? Height { get; set; }

        [Display(Name = "Вес")]
        public int? Weight { get; set; }

        public int? CityID { get; set; }

        public string CityName { get; set; }

    }
}