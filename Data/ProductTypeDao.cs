using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ProductTypeDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public List<ProducType> ListProductType()
        {
            var res = data.ProducTypes.ToList();
            return res;
        }
    }
}
