﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public Nullable<int> User_id { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNum{ get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}