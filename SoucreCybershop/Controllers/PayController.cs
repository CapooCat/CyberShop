using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Models;
namespace CyberShop.Controllers
{
    public class PayController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Pay
        public ActionResult Index()
        {
            var model = new CustomerOrderViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(CustomerOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Session[CyberShop.Common.CommonConstantUser.CART_SESSION] == null)
                {
                    ModelState.AddModelError("", "Không thể thanh toán vì không có sản phẩm");
                }
                else
                {
                    //tạo hóa đơn
                    InvoiceDao inDao = new InvoiceDao();
                    var invoice = new Invoice();
                    var user_session = (CyberShop.Common.UserInfo)Session[CyberShop.Common.CommonConstantUser.USER_SESSION];
                    if (Session[CyberShop.Common.CommonConstantUser.USER_SESSION] != null)
                    {
                        invoice.User_id = user_session.Id;
                    }
                    else
                    {
                        invoice.User_id = null;
                        invoice.CustomerName = model.CustomerName;
                    }
                    invoice.PurchaseDate = DateTime.Now;
                    invoice.DeliveryAddress = model.Address + " " + model.City;
                    invoice.DeliveryPhoneNum = model.PhoneNum;
                    invoice.Status = "Đã hoàn thành";
                    invoice.IsDeleted = false;
                    invoice.CreateBy = "Admin";
                    invoice.CreateDate = DateTime.Now;
                    List<CartViewModel> cart = (List<CartViewModel>)Session[CyberShop.Common.CommonConstantUser.CART_SESSION];
                    double? total = 0;
                    foreach (CartViewModel item in cart)
                    {
                        total += item.Price;
                    }
                    invoice.Total = total;
                    inDao.InsertInvoice(invoice);
                    //Tạo chi tiết hóa đơn
                    Invoice_Detail inDetail = new Invoice_Detail();
                    InvoiceDetailDao inDetailDao = new InvoiceDetailDao();
                    foreach (CartViewModel item in cart)
                    {
                        inDetail.Invoice_id = data.Invoices.OrderByDescending(x => x.Id).First().Id;
                        inDetail.Product_id = item.id;
                        inDetail.Amount = item.Quanlity;
                        inDetail.Price = item.Price;
                        var warranty = Convert.ToInt32(data.Products.First(x => x.id == item.id).MonthWarranty);
                        inDetail.WarrantyExpires = DateTime.Now.AddMonths(warranty);
                        inDetail.CreateBy = "Admin";
                        inDetail.CreateDate = DateTime.Now;
                        inDetailDao.InsertInvoiceDetail(inDetail);
                    }
                    cart.RemoveAll(x => cart.Any());
                    ViewBag.Success = "yes";
                }
            }
            return View(model);
        }
        //public JsonResult ConfirmCoupon(string id)
        //{
        //    var coupon = data.Coupons.Where(x => x.Coupon_Code.Equals(id)).Count();
        //    if (coupon > 0)
        //    {
        //        var priceDiscount = data.Coupons.Where(x => x.Coupon_Code == id).Select(x => x.Money_Discount);
        //        return Json(new { success = "true", price_discount = priceDiscount }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { success = "false" }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}