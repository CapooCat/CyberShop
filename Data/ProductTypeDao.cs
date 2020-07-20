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
            var res = data.ProducTypes.Where(x => x.IsDeleted == false).ToList();
            return res;
        }

        public bool InsertType(ProducType entity)
        {
            if (entity != null)
            {
                data.ProducTypes.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
