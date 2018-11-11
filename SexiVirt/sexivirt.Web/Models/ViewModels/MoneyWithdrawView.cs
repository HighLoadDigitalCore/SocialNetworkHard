using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;


namespace sexivirt.Web.Models.ViewModels
{ 
	public class MoneyWithdrawView
    {

        public int ID { get; set; }

		public int UserID {get; set; }

		public int? MoneyDetailID {get; set; }

		public double Sum {get; set; }

		public int Provider {get; set; }

		public string Account {get; set; }

		public DateTime AddedDate {get; set; }

		public bool Submitted {get; set; }

    }
}