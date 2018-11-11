using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Models.ViewModels.Info
{
    public class EventSearch
    {
        public bool IsReal { get; set; }

        public int? DayStart { get; set; }

        public int? DayEnd { get; set; }

        public IEnumerable<SelectListItem> DayStartSelectList
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "Неважно",
                    Selected = !DayStart.HasValue
                };
                for (int i = 0; i <= 31; i++)
                {
                    string text = null;
                    if (i == 0)
                    {
                        text = "Сегодня";
                    }
                    else if (i == 1)
                    {
                        text = "Завтра";
                    }
                    else if (i == 2)
                    {
                        text = "Послезавтра";
                    }
                    else
                    {
                        text = DateTime.Now.AddDays(i).ToString("dd MMMM");
                    }
                    yield return new SelectListItem()
                    {
                        Value = i.ToString(),
                        Text = text,
                        Selected = DayStart == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> DayEndSelectList
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "Неважно",
                    Selected = !DayStart.HasValue
                };
                for (int i = 0; i <= 31; i++)
                {
                    string text = null;
                    if (i == 0)
                    {
                        text = "Сегодня";
                    }
                    else if (i == 1)
                    {
                        text = "Завтра";
                    }
                    else if (i == 2)
                    {
                        text = "Послезавтра";
                    }
                    else
                    {
                        text = DateTime.Now.AddDays(i).ToString("dd MMMM");
                    }
                    yield return new SelectListItem()
                    {
                        Value = i.ToString(),
                        Text = text,
                        Selected = DayEnd == i
                    };
                }
            }
        }

    }
}