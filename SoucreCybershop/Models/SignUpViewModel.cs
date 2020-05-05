using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CyberShop.Models
{
    public class SignUpViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên mật khẩu")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        public string Re_Password { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng")]
        public string Email { get; set; }
        public string Name { get; set; }
        public System.DateTime DayOfBirth { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}