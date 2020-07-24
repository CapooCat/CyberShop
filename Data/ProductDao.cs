using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ProductDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public List<Product> ListProduct()
        {
            var res = data.Products.Where(x => x.IsDeleted == false).ToList();
            return res;
        }
        public bool InsertProduct(Product entity)
        {
            if (entity != null)
            {
                data.Products.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
