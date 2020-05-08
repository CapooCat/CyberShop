using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class UserViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        [Display(Name = "Tên tài khoản ")]
        public string Username { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [Display(Name = "Email ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa chỉ ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Tỉnh/Thành không được để trống")]
        [Display(Name = "Tỉnh thành ")]
        public string City { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Display(Name = "Số điện thoại ")]
        public string PhoneNum { get; set; }
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ tên")]
        public string Name { get; set; }
        public System.DateTime DayOfBirth { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}