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
                
                if(Session[CyberShop.Common.CommonConstantUser.CART_SESSION]==null)
                {
                    ModelState.AddModelError("", "Không thể thanh toán vì không có sản phẩm");
                }
                else
                {
                    CustomerOrderDao ctrDao = new CustomerOrderDao();
                    var customerOrder = new CustomerOrder();
                    customerOrder.CustomerName = model.CustomerName;
                    customerOrder.Address = model.Address;
                    customerOrder.Email = model.Email;
                    customerOrder.PhoneNum = model.PhoneNum;
                    customerOrder.IsDeleted = true;
                    customerOrder.CreateBy = "Admin";
                    customerOrder.CreateDate = DateTime.Now;
                    ctrDao.InsertCustomerOrDer(customerOrder);

                    InvoiceDao inDao = new InvoiceDao();
                    customerOrder.Id = data.CustomerOrders.First(x=>x.PhoneNum==model.PhoneNum).Id;
                    var invoice = new Invoice();
                    invoice.CustomerOrder_id=customerOrder.Id;
                    invoice.DeliveryAddress = model.Address +" "+ model.City;
                    invoice.DeliveryPhoneNum = model.PhoneNum;
                    List<CartViewModel> cart = (List<CartViewModel>)Session[CyberShop.Common.CommonConstantUser.CART_SESSION];
                    double? total = 0;
                    foreach(CartViewModel item in cart)
                    {
                        total += item.Price;
                    }
                    invoice.Total = total;
                    inDao.InsertInvoice(invoice);
                }
            }
            return View(model);
        }
    }
}