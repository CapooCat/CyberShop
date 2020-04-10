using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CyberShop.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; }
    }
}