using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class InvoiceDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertInvoice(Invoice entity)
        {
            if (entity != null)
            {
                data.Invoices.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
