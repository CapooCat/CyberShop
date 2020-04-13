using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data
{
    public class CategoriesDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public List<Categoryy> getAllCatgories()
        {
               return data.Categoryies.ToList();
        }
    }
}
