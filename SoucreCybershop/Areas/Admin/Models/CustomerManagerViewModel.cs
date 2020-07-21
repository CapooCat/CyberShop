using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Areas.Admin.Models
{
    public class CustomerManagerViewModel
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreatedDate { get; set; }
    }
}