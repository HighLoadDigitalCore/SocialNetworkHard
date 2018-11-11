using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{

    public partial class SqlRepository
    {
        public int CreateRoboTransaction(decimal sum, int userID)
        {
            var transaction = new RoboTransaction()
            {
                Date = DateTime.Now,
                Status = -1,
                Sum = sum,
                UserID = userID

            };
            Db.RoboTransactions.InsertOnSubmit(transaction);
            Db.SubmitChanges();
            return transaction.ID;
        }

        public RoboTransaction GetRoboTransaction(int id)
        {
            return Db.RoboTransactions.FirstOrDefault(x=> x.ID == id);
        }

        public bool SubmitRoboTransaction(RoboTransaction order)
        {
            var cache = Db.RoboTransactions.FirstOrDefault(x => x.ID == order.ID);
            if (cache != null)
            {
                cache.Status = 1;
                if (!cache.MoneyDetailID.HasValue)
                {
                    var money = new MoneyDetail()
                    {
                        AddedDate = order.Date,
                        Description = "Пополнение счета",
                        Global = Guid.NewGuid(),
                        IsFee = false,
                        Sum = (double) order.Sum,
                        UserID = order.UserID,
                        Type = (int) MoneyDetail.TypeEnum.Income,
                        Submited = true
                    };
                    Db.MoneyDetails.InsertOnSubmit(money);
                    Db.MoneyDetails.Context.SubmitChanges();
                    cache.MoneyDetailID = money.ID;
                }
                Db.RoboTransactions.Context.SubmitChanges();
                RecalculateUserMoney(cache.UserID);
                return true;
            }


            return false;
        }  
        
        public bool FailRoboTransaction(RoboTransaction order)
        {
            var cache = Db.RoboTransactions.FirstOrDefault(x => x.ID == order.ID);
            if (cache != null)
            {
                cache.Status = -1;
                if (cache.MoneyDetailID.HasValue)
                {
                    var money = Db.MoneyDetails.FirstOrDefault(x => x.ID == cache.MoneyDetailID);

                    cache.MoneyDetailID = null;
                    Db.RoboTransactions.Context.SubmitChanges();
                    
                    if (money != null)
                    {
                        Db.MoneyDetails.DeleteOnSubmit(money);
                        Db.MoneyDetails.Context.SubmitChanges();
                        
                    }
                }
                
                RecalculateUserMoney(cache.UserID);
                return true;
            }


            return false;
        }
    }
}