using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class InOrderDetailDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertInOrderDetail(InOrder_Detail entity)
        {
            if (entity != null)
            {
                data.InOrder_Detail.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
