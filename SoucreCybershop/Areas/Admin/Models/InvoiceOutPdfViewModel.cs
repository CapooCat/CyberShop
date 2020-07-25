using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class InvoiceOutPdfViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeName { get; set; }
        public string CustomerName { get; set; }
        public string PhoneName { get; set; }
        public string Address { get; set; }
        public int? MonthsWarranty { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}