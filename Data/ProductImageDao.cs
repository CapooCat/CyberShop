using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ProductImageDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertImage(ProductImage entity)
        {
            if (entity != null)
            {
                data.ProductImages.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
