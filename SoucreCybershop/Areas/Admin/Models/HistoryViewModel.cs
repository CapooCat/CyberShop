using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public Nullable<int> User_id { get; set; }
        public string Description { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductName { get; set; }
        public Nullable<bool> Product_tag { get; set; }
        public Nullable<bool> Invoice_tag { get; set; }
        public Nullable<bool> Warehouse_tag { get; set; }
        public Nullable<bool> User_tag { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Product_id { get; set; }
        public Nullable<int> Invoice_id { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<double> TotalPrice { get; set; }
    }
}