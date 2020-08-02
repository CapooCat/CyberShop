using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Product_id { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<double> TotalPrice { get; set; }
    }
}