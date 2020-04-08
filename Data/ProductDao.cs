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
            var res = data.Products.ToList();
            return res;
        }
    }
}
