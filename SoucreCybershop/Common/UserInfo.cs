using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Common
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string TaiKhoan { get; set; }
        public string Image { get; set; }
        public string HoTen { get; set; }
        public string DateOfBirth { get; set; }
    }
}