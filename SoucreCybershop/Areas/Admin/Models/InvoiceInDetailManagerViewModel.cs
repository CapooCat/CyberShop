using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class InvoiceInDetailManagerViewModel
    {
        public int Id { get; set; }
        public int? InOrder_id { get; set; }
        public int? Product_id { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ProductName { get; set; }
        public double? TotalPrice { get; set; }
        public string Status { get; set; }
    }
}