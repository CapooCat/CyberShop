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
        public List<Category> getAllCatgories()
        {
            return data.Categories.Where(x => x.category_lv2_id == null && x.category_lv3_id==null && x.IsDeleted == false).ToList();
        }
        public List<Category> getListCategoryLv2(int categoryLv1_id)
        {
            return data.Categories.Where(x => x.category_lv1_master_id == categoryLv1_id && x.category_lv3_id==null && x.IsDeleted == false).ToList();
        }
        public List<Category> getListCategoryLv3(int? categoryLv2_id)
        {
            return data.Categories.Where(x => x.category_lv2_master_id == categoryLv2_id && x.IsDeleted == false).ToList();
        }
        public bool InsertCategoryLv1(Category entity)
        {
            if (entity != null)
            {
                data.Categories.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateCategory(Category entity)
        {
            if (entity != null)
            {
                data.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
