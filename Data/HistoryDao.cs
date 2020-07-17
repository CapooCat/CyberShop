using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class HistoryDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertHistory(History entity)
        {
            if (entity != null)
            {
                data.Histories.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
