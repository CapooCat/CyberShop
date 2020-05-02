using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CyberShop.Models
{
    public class CustomerOrderViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        public string Address { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn phương thức thanh toán")]
        public string Payment { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn phương thức nhận hàng")]
        public string Receive { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        public string PhoneNum { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}