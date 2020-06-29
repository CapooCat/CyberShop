using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BrandDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public List<Brand> ListBrand()
        {
            var res = data.Brands.OrderByDescending(x=>x.BrandName).ToList();
            return res;
        }
    }
}
