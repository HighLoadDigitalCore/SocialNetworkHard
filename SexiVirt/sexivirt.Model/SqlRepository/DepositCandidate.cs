using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace sexivirt.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<DepositCandidate> DepositCandidates
        {
            get
            {
                return Db.DepositCandidates;
            }
        }

        public bool CreateDepositCandidate(DepositCandidate instance)
        {
            if (instance.ID == 0)
            {
                Db.DepositCandidates.InsertOnSubmit(instance);
                Db.DepositCandidates.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateDepositCandidate(DepositCandidate instance)
        {
            var cache = Db.DepositCandidates.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.AddedDate = instance.AddedDate;
				cache.Description = instance.Description;
				cache.Sum = instance.Sum;
                Db.DepositCandidates.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveDepositCandidate(int idDepositCandidate)
        {
            DepositCandidate instance = Db.DepositCandidates.FirstOrDefault(p => p.ID == idDepositCandidate);
            if (instance != null)
            {
                Db.DepositCandidates.DeleteOnSubmit(instance);
                Db.DepositCandidates.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}