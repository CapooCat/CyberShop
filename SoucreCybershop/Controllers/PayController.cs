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
            var user_session = (CyberShop.Common.UserInfo)Session[CyberShop.Common.CommonConstantUser.USER_SESSION];
            if (Session[CyberShop.Common.CommonConstantUser.USER_SESSION] != null)
            {
                model = data.Users.Where(x => x.id == user_session.Id).Select(x => new CustomerOrderViewModel
                {
                    CustomerName = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    PhoneNum = x.PhoneNum
                }).FirstOrDefault();
            }
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
                    else if (model.Address == "" || model.PhoneNum == "" || model.CustomerName == "" || model.Email == "")
                    {
                        ModelState.AddModelError("", "Không được bỏ trống");
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
                            invoice.CustomerName = model.CustomerName;
                        }
                        else
                        {
                            invoice.User_id = null;
                            invoice.CustomerName = model.CustomerName;
                        }
                        invoice.PurchaseDate = DateTime.Now;
                        invoice.DeliveryAddress = model.Address + " " + model.City;
                        invoice.DeliveryPhoneNum = model.PhoneNum;
                        invoice.Status = "Đang chờ xem";
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
                            if (item.ProductType == 20)
                            {
                                var PC = new PCViewModel();
                                PC = data.Detail_PcSets.Where(x => x.product_id == item.id).Select(x => new PCViewModel
                                {
                                    cpu_id = x.cpu_id,
                                    main_id = x.main_id,
                                    ram_id = x.ram_id,
                                    hdd_id = x.hdd_id,
                                    ssd_id = x.ssd_id,
                                    vga_int = x.vga_int,
                                    power_id = x.power_id,
                                    cooler_id = x.cooler_id,
                                    monitor_id = x.monitor_id,
                                    case_id = x.case_id
                                }).FirstOrDefault();

                                AddToInvoice(PC.cpu_id, item.Quanlity);
                                AddToInvoice(PC.main_id, item.Quanlity);
                                AddToInvoice(PC.ram_id, item.Quanlity);
                                AddToInvoice(PC.hdd_id, item.Quanlity);
                                AddToInvoice(PC.ssd_id, item.Quanlity);
                                AddToInvoice(PC.vga_int, item.Quanlity);
                                AddToInvoice(PC.power_id, item.Quanlity);
                                AddToInvoice(PC.cooler_id, item.Quanlity);
                                AddToInvoice(PC.monitor_id, item.Quanlity);
                                AddToInvoice(PC.case_id, item.Quanlity);
                            }
                            else
                            {
                                inDetail.Invoice_id = data.Invoices.OrderByDescending(x => x.Id).First().Id;
                                inDetail.Product_id = item.id;
                                inDetail.Amount = item.Quanlity;
                                inDetail.Price = item.Price;
                                var warranty = Convert.ToInt32(data.Products.First(x => x.id == item.id).MonthWarranty);
                                inDetail.WarrantyExpires = DateTime.Now.AddMonths(warranty);
                                inDetail.CreateBy = "Admin";
                                inDetail.CreateDate = DateTime.Now;
                                inDetail.IsDeleted = false;
                                inDetailDao.InsertInvoiceDetail(inDetail);
                            }
                        }
                        cart.RemoveAll(x => cart.Any());
                        ViewBag.Success = "yes";
                    }
                }
            
            return View(model);
        }

        public bool AddToInvoice(Nullable<int> id, int quantity)
        {
            if (id == null)
            {
                return false;
            }
            else
            {
                Invoice_Detail inDetail = new Invoice_Detail();
                InvoiceDetailDao inDetailDao = new InvoiceDetailDao();
                inDetail.Invoice_id = data.Invoices.OrderByDescending(x => x.Id).First().Id;
                inDetail.Product_id = id.GetValueOrDefault();
                inDetail.Amount = quantity;
                inDetail.Price = Convert.ToDouble(data.Products.First(x => x.id == id).Price);
                var warranty = Convert.ToInt32(data.Products.First(x => x.id == id).MonthWarranty);
                inDetail.WarrantyExpires = DateTime.Now.AddMonths(warranty);
                inDetail.CreateBy = "Admin";
                inDetail.CreateDate = DateTime.Now;
                inDetail.IsDeleted = false;
                inDetailDao.InsertInvoiceDetail(inDetail);
                return true;
            }
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