using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{

    public partial class SqlRepository
    {
        public IQueryable<MoneyDetail> MoneyDetails
        {
            get
            {
                return Db.MoneyDetails;
            }
        }

        public Guid CreateTripleMoneyDetail(MoneyDetail from, MoneyDetail to, MoneyDetail fee = null)
        {
            var guid = Guid.NewGuid();
            CreateMoneyDetail(from, guid);
            CreateMoneyDetail(to, guid);
            if (fee != null)
            {
                CreateMoneyDetail(fee, guid);
            }
            return guid;
        }

        public bool CreateMoneyDetail(MoneyDetail instance, Guid uniqueGuid)
        {
            instance.Global = uniqueGuid;
            instance.AddedDate = DateTime.Now;
            Db.MoneyDetails.InsertOnSubmit(instance);
            Db.MoneyDetails.Context.SubmitChanges();
            if (instance.UserID.HasValue && instance.Submited)
            {
                RecalculateUserMoney(instance.UserID.Value);
            }
            return true;
        }

        public bool SubmitMoney(Guid guid)
        {
            var moneyDetails = Db.MoneyDetails.Where(p => p.Global == guid);
            if (moneyDetails.Any())
            {
                foreach (var moneyDetail in moneyDetails.ToList())
                {
                    var cache = Db.MoneyDetails.FirstOrDefault(p => p.ID == moneyDetail.ID);
                    cache.Submited = true;
                    Db.MoneyDetails.Context.SubmitChanges();
                    if (cache.UserID.HasValue)
                    {
                        RecalculateUserMoney(cache.UserID.Value);
                    }
                }
                return true;
            }
            return false;
        }

        public bool DiscardMoney(Guid guid)
        {
            var moneyDetails = Db.MoneyDetails.Where(p => p.Global == guid);
            if (moneyDetails.Any())
            {
                Db.MoneyDetails.DeleteAllOnSubmit(moneyDetails.ToList());
                Db.MoneyDetails.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public void RecalculateUserMoney(int userID)
        {
            Db.Users.Context.SubmitChanges();
            double totalSum = 0;
            if (MoneyDetails.Any(p => p.UserID == userID && p.Submited))
            {
                totalSum = MoneyDetails.Where(p => p.UserID == userID && p.Submited).Sum(p => p.Sum);
            }
            var user = Users.FirstOrDefault(p => p.ID == userID);
            
            if (user != null)
            {
                user.Money = totalSum;
                Db.Users.Context.SubmitChanges();
            }
        }

        public void RecalculateAll()
        {
            foreach (var userID in Users.Select(p => p.ID).ToList())
            {
                RecalculateUserMoney(userID);
            }
        }

        public void RemoveUnsubmitted()
        {
            var moneyDetails = Db.MoneyDetails.Where(p => !p.Submited).ToList();
            Db.MoneyDetails.DeleteAllOnSubmit(moneyDetails);
            Db.MoneyDetails.Context.SubmitChanges();
        }

        public void RemoveMoneyTransaction(Guid guid)
        {
            var moneyDetails = Db.MoneyDetails.Where(p => p.Global == guid).ToList();
            Db.MoneyDetails.DeleteAllOnSubmit(moneyDetails);
            Db.MoneyDetails.Context.SubmitChanges();
        }
    }
}