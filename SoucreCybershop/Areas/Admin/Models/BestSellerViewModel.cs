using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class BestSellerViewModel
    {
        public int Product_Id { get; set; }
        public int Invoice_Id { get; set; } 
        public string ProductName { get; set; }
        public int? SellAmount { get; set; }
        public double? Price { get; set; }
    }
}