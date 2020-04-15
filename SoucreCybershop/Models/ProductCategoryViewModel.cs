using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class ProductCategoryViewModel
    {
        public int id { get; set; }
        public int Brand_id { get; set; }
        public Nullable<int> Promotion_id { get; set; }
        public string ProductName { get; set; }
        public string Info { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> MonthWarranty { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int ProductType_id { get; set; }
    }
}