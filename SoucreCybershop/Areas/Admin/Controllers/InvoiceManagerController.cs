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
        public JsonResult ReturnInvoice()
        {
            var model = new List<InvoiceManagerViewModel>();
            model = data.Invoices.Select(x => new InvoiceManagerViewModel
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
                if (item.User_id == null)
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
                else
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
                        CustomerName = data.Users.Where(x => x.id == item.User_id).First().Name 
                    });
                }
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