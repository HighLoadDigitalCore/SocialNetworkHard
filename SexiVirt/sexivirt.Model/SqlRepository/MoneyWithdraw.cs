using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{

    public partial class SqlRepository
    {
        public IQueryable<MoneyWithdraw> MoneyWithdraws
        {
            get
            {
                return Db.MoneyWithdraws;
            }
        }

        public bool CreateMoneyWithdraw(MoneyWithdraw instance)
        {
            if (instance.ID == 0)
            {
                Db.MoneyWithdraws.InsertOnSubmit(instance);
                Db.MoneyWithdraws.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMoneyWithdraw(MoneyWithdraw instance)
        {
            var cache = Db.MoneyWithdraws.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.UserID = instance.UserID;
                cache.MoneyDetailID = instance.MoneyDetailID;
                cache.Sum = instance.Sum;
                cache.Provider = instance.Provider;
                cache.Account = instance.Account;
                cache.AddedDate = instance.AddedDate;
                cache.Submitted = instance.Submitted;
                Db.MoneyWithdraws.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        public bool UpdateMoneyWithdrawStatus(MoneyWithdraw instance)
        {
            var cache = Db.MoneyWithdraws.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Submitted = instance.Submitted;
                cache.MoneyDetail.Submited = instance.Submitted;
                Db.MoneyWithdraws.Context.SubmitChanges();
                RecalculateUserMoney(cache.UserID);
                return true;
            }

            return false;
        }

        public bool RemoveMoneyWithdraw(int idMoneyWithdraw)
        {
            MoneyWithdraw instance = Db.MoneyWithdraws.FirstOrDefault(p => p.ID == idMoneyWithdraw);
            if (instance != null)
            {
                Db.MoneyWithdraws.DeleteOnSubmit(instance);
                Db.MoneyWithdraws.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}