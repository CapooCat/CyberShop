using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class CartViewModel
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public Nullable<double> Price { get; set; }
        public int Quanlity { get; set; }
        public string Image { get; set; }
    }
}