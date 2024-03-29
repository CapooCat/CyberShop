﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class CategoryManagerViewModel
    {
        public int Id { get; set; }
        public Nullable<int> category_lv2_id { get; set; }
        public Nullable<int> category_lv3_id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Metatitle { get; set; }
        public Nullable<int> category_lv1_master_id { get; set; }
        public Nullable<int> category_lv2_master_id
        {
            get; set;
        }
        public string ProductTypeKW { get; set; }
        public string BrandKW { get; set; }
        public string ProductKW { get; set; }
        public int? LowPrice { get; set; }
        public int? HighPrice { get; set; }
        public string CateNameLv1 { get; set; }
        public string CateNameLv2 { get; set; }
    }
}