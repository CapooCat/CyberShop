using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class InvoiceDetailViewModel
    {
        public int Id { get; set; }
        public int Invoice_id { get; set; }
        public int Product_id { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<double> Price { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> WarrantyExpires { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}