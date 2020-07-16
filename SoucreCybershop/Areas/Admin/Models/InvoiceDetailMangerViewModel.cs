using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class InvoiceDetailMangerViewModel
    {
        public int Id { get; set; }
        public int Invoice_id { get; set; }
        public int Product_id { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> WarrantyExpires { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ProductName { get; set; }
    }
}