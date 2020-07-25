using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class DetailProductViewModel
    {
        public int id { get; set; }
        public int Brand_id { get; set; }
        public int ProductType_id { get; set; }
        public Nullable<int> Promotion_id { get; set; }
        public int Categorie_id { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Amount { get; set; }
        public string Info { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> MonthWarranty { get; set; }
        public string Url { get; set; }

    }
}