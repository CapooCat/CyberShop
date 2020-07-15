using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class ProductManagerViewModel
    {
        public int? id { get; set; }
        public int Brand_id { get; set; }
        public int? ProductType_id { get; set; }
        public string ProductName { get; set; }
        public string MetaTitle { get; set; }
        public string Info { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> SalePercent { get; set; }
        public Nullable<int> MonthWarranty { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Amount { get; set; }
        public string ProductTypeName { get; set; }
        public string BrandName { get; set; }
        public float? PriceTo { get; set; }
        public float? PriceFrom { get; set; }
    }
}