using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CustomerOrderDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        //public bool InsertCustomerOrDer(CustomerOrder entity)
        //{
        //    try
        //    {
        //        if (entity != null)
        //        {
        //            data.CustomerOrders.Add(entity);
        //            data.SaveChanges();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }
        //}
    }
}
