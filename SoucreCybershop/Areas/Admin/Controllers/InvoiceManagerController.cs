using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
namespace CyberShop.Areas.Admin.Controllers
{
    public class InvoiceManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/InvoiceManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddProductToInvoice(InvoiceDetailMangerViewModel model)
        {
            var InvoiceDetailDao = new InvoiceDetailDao();
            var InvoiceDetail = new Invoice_Detail();

            var list = new List<InvoiceDetailMangerViewModel>();
            if (list.Exists(x => x.Product_id == model.Product_id))
            {
                foreach (var item in list)
                {
                    if (item.Product_id == model.Product_id)
                    {
                        item.Amount += 1;
                    }
                }
            }
            else
            {
                InvoiceDetail.Invoice_id = model.Invoice_id;
                InvoiceDetail.Product_id = model.Product_id;
                InvoiceDetail.Price = model.Price;
                InvoiceDetail.Amount = 1;
                InvoiceDetail.IsDeleted = false;
                InvoiceDetail.CreateDate = DateTime.Now;
                InvoiceDetail.CreateBy = "Admin";
            }
            if (InvoiceDetailDao.InsertInvoiceDetail(InvoiceDetail))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        public JsonResult ReturnInvoice()
        {
            var model = new List<InvoiceManagerViewModel>();
            model = data.Invoices.Where(x=>x.IsDeleted==false).Select(x => new InvoiceManagerViewModel
            {
                Id = x.Id,
                User_id=x.User_id,
                BuyDate=x.PurchaseDate.ToString(),
                PurchaseDate=x.PurchaseDate,
                DeliveryAddress=x.DeliveryAddress,
                DeliveryPhoneNum=x.DeliveryPhoneNum,
                Status=x.Status,
                Total=x.Total,
                IsDeleted=x.IsDeleted,
                CreateBy=x.CreateBy,
                CreateDate=x.CreateDate,
                CustomerName=x.CustomerName
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
                        CustomerName =item.CustomerName
                    });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnInvoiceById(int id)
        {
            var model = new List<InvoiceManagerViewModel>();
            model = data.Invoices.Where(x => x.IsDeleted == false && x.Id==id).Select(x => new InvoiceManagerViewModel
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
                     where a.IsDeleted==false && a.Invoice_id==id
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
        [HttpPost]
        public JsonResult FilterInvoice(InvoiceManagerViewModel model)
        {
            var lstInvoice = new List<InvoiceManagerViewModel>();
            lstInvoice = data.Invoices.Where(x => ((model.Id == null) || (x.Id == model.Id)))
                                      .Where(x => ((model.CustomerName == null) || (x.CustomerName.Contains(model.CustomerName))))
                                      .Where(x => ((model.DeliveryPhoneNum == null) || (x.DeliveryPhoneNum == model.DeliveryPhoneNum)))
                .Select(x => new InvoiceManagerViewModel {
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
            if (model.DateFrom != null && model.DateTo==null)
            {
                var dateFrom = DateTime.Parse(model.DateFrom);
                lstInvoice = lstInvoice.Where(x=>x.PurchaseDate == dateFrom).ToList();
            }
            if(model.DateFrom == null && model.DateTo != null)
            {
                var dateTo = DateTime.Parse(model.DateTo);
                lstInvoice = lstInvoice.Where(x => x.PurchaseDate == dateTo).ToList();
            }
            if(model.DateFrom != null && model.DateTo != null)
            {
                var dateFrom = DateTime.Parse(model.DateFrom);
                var dateTo = DateTime.Parse(model.DateTo);
                lstInvoice = lstInvoice.Where(x => x.PurchaseDate >= dateFrom && x.PurchaseDate <= dateTo).ToList();
            }
            lstInvoice = lstInvoice.Where(x => x.IsDeleted == false).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in lstInvoice)
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
    }
}