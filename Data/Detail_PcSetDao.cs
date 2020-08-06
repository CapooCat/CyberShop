using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Detail_PcSetDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool InsertPC(Detail_PcSets entity)
        {
            if (entity != null)
            {
                data.Detail_PcSets.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
