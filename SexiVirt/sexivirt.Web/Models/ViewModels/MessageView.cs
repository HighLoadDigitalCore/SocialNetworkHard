using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sexivirt.Model;


namespace sexivirt.Web.Models.ViewModels
{ 
	public class MessageView
    {

        public int ID { get; set; }

		public int SenderID {get; set; }

		public int ReceiverID {get; set; }

		public DateTime AddedDate {get; set; }

		public string Text {get; set; }

		public bool IsSend {get; set; }

		public DateTime? ReadedDate {get; set; }

		public bool IsDeleted {get; set; }

    }
}