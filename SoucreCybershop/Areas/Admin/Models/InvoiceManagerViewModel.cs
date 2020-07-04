using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class InvoiceManagerViewModel
    {
        public int? Id { get; set; }
        public Nullable<int> User_id { get; set; }
        public string PurchaseDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryPhoneNum { get; set; }
        public string Status { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}