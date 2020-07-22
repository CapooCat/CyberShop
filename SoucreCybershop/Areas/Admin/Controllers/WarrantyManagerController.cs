using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;

namespace CyberShop.Areas.Admin.Controllers
{
    public class WarrantyManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/WarrantyManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReturnInvoiceById(int id)
        {
            var model = new List<InvoiceManagerViewModel>();
            model = data.Invoices.Where(x => x.IsDeleted == false && x.Id == id).Select(x => new InvoiceManagerViewModel
            {
                Id = x.Id,
                User_id = x.User_id,
                BuyDate = x.PurchaseDate.ToString(),
                PurchaseDate = x.PurchaseDate,
                DeliveryAddress = x.DeliveryAddress,
                DeliveryPhoneNum = x.DeliveryPhoneNum,
                Status = x.Status,
                Total = x.Total,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                CustomerName = x.CustomerName
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceManagerViewModel
                {
                    Id = item.Id,
                    User_id = item.User_id,
                    BuyDate = item.BuyDate,
                    PurchaseDate = item.PurchaseDate,
                    DeliveryAddress = item.DeliveryAddress,
                    DeliveryPhoneNum = item.DeliveryPhoneNum,
                    Status = item.Status,
                    Total = item.Total,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    CustomerName = item.CustomerName
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnDetailInvoiceById(int id)
        {
            var model = new List<InvoiceDetailMangerViewModel>();
            model = (from a in data.Invoice_Detail
                     join b in data.Products on a.Product_id equals b.id
                     where a.IsDeleted == false && a.Invoice_id == id
                     select new InvoiceDetailMangerViewModel
                     {
                         Id = a.Id,
                         Invoice_id = a.Invoice_id,
                         Product_id = a.Product_id,
                         ProductName = b.ProductName,
                         Amount = a.Amount,
                         Price = a.Price,
                         WarrantyExpires = a.WarrantyExpires,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceDetailMangerViewModel
                {
                    Id = item.Id,
                    Invoice_id = item.Invoice_id,
                    Product_id = item.Product_id,
                    ProductName = item.ProductName,
                    Amount = item.Amount,
                    Price = item.Price,
                    WarrantyExpires = item.WarrantyExpires,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}