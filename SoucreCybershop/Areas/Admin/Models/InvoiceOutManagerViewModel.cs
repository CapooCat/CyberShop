using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class InvoiceOutManagerViewModel
    {
        public int? Product_Id { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string ProductTypeName { get; set; }
        public double? Price { get; set; }
        public int Amount { get; set; }
        public int? WarrantyMonths { get; set; }
    }
}