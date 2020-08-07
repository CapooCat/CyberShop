using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class ProductTypeManagerViewModel
    {
        public int? id { get; set; }
        public string TypeName { get; set; }
        public string MetaTitle { get; set; }
        public int? Category_id { get; set; }
        public string CategoryName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}