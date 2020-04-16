using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CategoryDetailDao
    {
            ShopPCComponentsEntities data = new ShopPCComponentsEntities();

        public int Id { get; set; }
        public string CateDetailName { get; set; }
        public int Category_id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        public List<CategoryDetailDao> getListCateDetail(int category_id)
        {
            return (from a in data.Categoryies
                    join b in data.CategoryDetails on a.Id equals b.Category_id
                    where a.Id == category_id
                    select new CategoryDetailDao
                    {
                        Id=b.Id,
                        CateDetailName=b.CateDetailName,
                        Category_id=b.Id,
                        IsDeleted=b.IsDeleted,
                        CreateBy=b.CreateBy,
                        CreateDate=b.CreateDate,
                        ModifiedBy=b.ModifiedBy,
                        ModifiedTime=b.ModifiedTime
                    }).ToList();
        }
        public List<CategoryDetailSuggest> getListCateDetailSuggest(int categoryDetail_id)
        {
            return data.CategoryDetailSuggests.Where(x => x.CategoryDetail_id == categoryDetail_id).ToList();
        }
    }
}
