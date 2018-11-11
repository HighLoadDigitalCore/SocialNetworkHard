using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sexivirt.Model
{
    public partial class SqlRepository
    {
        public IQueryable<MessagePayment> MessagePayments
        {
            get { return Db.MessagePayments; }
        }


        public bool UpdatePayment(MessagePayment payment)
        {
            var p = Db.MessagePayments.FirstOrDefault(x => x.ID == payment.ID);
            if (p != null)
            {
                p.Duration = payment.Duration;
                p.Price = payment.Price;
                Db.MessagePayments.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool CreatePayment(MessagePayment instance)
        {
            if (instance.ID == 0)
            {
                Db.MessagePayments.InsertOnSubmit(instance);
                Db.MessagePayments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public void DeleteMessagePayment(int id)
        {
            var p = Db.MessagePayments.FirstOrDefault(x => x.ID == id);
            if (p != null)
            {
                Db.MessagePayments.DeleteOnSubmit(p);
                Db.MessagePayments.Context.SubmitChanges();
            }
        }
    }
}
