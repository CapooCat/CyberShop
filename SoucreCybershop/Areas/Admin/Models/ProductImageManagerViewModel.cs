using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class ProductImageManagerViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Product_id { get; set; }

    }
}