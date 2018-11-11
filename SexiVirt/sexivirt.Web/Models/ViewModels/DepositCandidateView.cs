using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;


namespace sexivirt.Web.Models.ViewModels
{

    public class MoneyAddModel
    {
        public MoneyAddModel()
        {
            Sum = 500;
        }
        public string Error { get; set; }
        public decimal Sum { get; set; }
        public string RedirectLink { get; set; }
    }

    public class MoneyWithdrawModel
    {
        public string PayType { get; set; }
        public string PayAccount { get; set; }
        public string PaySum { get; set; }
    }

	public class DepositCandidateView
    {
        public int ID { get; set; }

		public int UserID {get; set; }

		public DateTime AddedDate {get; set; }

		public string Description {get; set; }

		public double Sum {get; set; }

    }
}