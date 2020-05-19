﻿using System;
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
                else if(data.Coupons.Where(x=>x.Coupon_Code.Equals(model.Discount)).Count() < 1)
                {
                    ModelState.AddModelError("", "Mã giảm giá không hợp lệ");
                }
                else
                {
                    //Tạo bảng khách hàng đặt sp
                    CustomerOrderDao ctrDao = new CustomerOrderDao();
                    var customerOrder = new CustomerOrder();
                    customerOrder.CustomerName = model.CustomerName;
                    customerOrder.Address = model.Address;
                    customerOrder.Email = model.Email;
                    customerOrder.PhoneNum = model.PhoneNum;
                    customerOrder.IsDeleted = false;
                    customerOrder.CreateBy = "Admin";
                    customerOrder.ModifiedBy = "";
                    customerOrder.CreateDate = DateTime.Now;
                    ctrDao.InsertCustomerOrDer(customerOrder);
                    //tạo hóa đơn
                    InvoiceDao inDao = new InvoiceDao();
                    customerOrder.Id = data.CustomerOrders.First(x => x.PhoneNum == model.PhoneNum).Id;
                    var invoice = new Invoice();
                    invoice.CustomerOrder_id = customerOrder.Id;
                    invoice.PurchaseDate = DateTime.Now;
                    invoice.DeliveryAddress = model.Address + " " + model.City;
                    invoice.DeliveryPhoneNum = model.PhoneNum;
                    invoice.Status = false;
                    invoice.IsDeleted = false;
                    invoice.CreateBy = "Admin";
                    invoice.CreateDate = DateTime.Now;
                    if(Session[Common.CommonConstantUser.USER_SESSION]!=null)
                    {
                        var userSession= (CyberShop.Common.UserInfo)Session[CyberShop.Common.CommonConstantUser.USER_SESSION];
                        invoice.User_id = userSession.Id;
                    }
                    else
                    {
                        invoice.User_id = null;
                    }
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
                    cart.RemoveAll(x=>cart.Any());
                    ViewBag.Success = "yes";
                }
            }
            return View(model);
        }
        public JsonResult ConfirmCoupon(string id)
        {
            var coupon = data.Coupons.Where(x => x.Coupon_Code.Equals(id)).Count();
            if (coupon > 0)
            {
                var priceDiscount = data.Coupons.Where(x => x.Coupon_Code == id).Select(x => x.Money_Discount);
                return Json(new { success = "true", price_discount = priceDiscount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = "false" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}