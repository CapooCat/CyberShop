using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class InvoiceDetailDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertInvoiceDetail(Invoice_Detail entity)
        {
            if (entity != null)
            {
                data.Invoice_Detail.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
