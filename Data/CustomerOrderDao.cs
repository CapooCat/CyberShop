using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CustomerOrderDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertCustomerOrDer(CustomerOrder entity)
        {
            if (entity != null)
            {
                data.CustomerOrders.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
