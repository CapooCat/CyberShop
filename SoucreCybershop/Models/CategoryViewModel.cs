using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string CateName { get; set; }

        public string breadcrumb { get; set; }
    }
}