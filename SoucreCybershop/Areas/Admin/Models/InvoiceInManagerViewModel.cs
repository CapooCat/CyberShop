using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class InvoiceInManagerViewModel
    {
        public int? Id { get; set; }
        public string CreateBy { get; set; }
        public string Info { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Status { get; set; }
    }
}