using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
using Rotativa;
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
        public JsonResult AddProductToInvoice(InvoiceDetailMangerViewModel model, int Product_Warranty)
        {
            var InvoiceDetailDao = new InvoiceDetailDao();
            var InvoiceDetail = new Invoice_Detail();
            var list = new List<InvoiceDetailMangerViewModel>();
            list =  (from a in data.Invoice_Detail
                     where a.Invoice_id == model.Invoice_id && a.Product_id == model.Product_id
                     select new InvoiceDetailMangerViewModel
                     {
                         Id = a.Id,
                         Invoice_id = a.Invoice_id,
                         Product_id = a.Product_id,
                         IsDeleted = a.IsDeleted

                     }).ToList();

            if (list.Any())
            {
                var n = list.First().Id;
                Invoice_Detail entity = new Invoice_Detail();
                if (list.First().IsDeleted == true)
                {
                    entity = data.Invoice_Detail.Find(n);
                    entity.Amount = 1;
                    entity.WarrantyExpires = DateTime.Now.AddMonths(Product_Warranty);
                    entity.IsDeleted = false;
                    data.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    entity = data.Invoice_Detail.Find(n);
                    entity.Amount += 1;
                    entity.WarrantyExpires = DateTime.Now.AddMonths(Product_Warranty);
                    data.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
                InvoiceDetail.WarrantyExpires = DateTime.Now.AddMonths(Product_Warranty);
                InvoiceDetail.CreateBy = "Admin";
                if (InvoiceDetailDao.InsertInvoiceDetail(InvoiceDetail))
                { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
                else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
            }
        }

        public JsonResult DeleteDetailInvoice(int id)
        {
            Invoice_Detail entity = new Invoice_Detail();
            entity = data.Invoice_Detail.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
                                      .Where(x => ((model.Status == null) || (x.Status == model.Status)))
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
                lstInvoice = lstInvoice.Where(x=>x.PurchaseDate >= dateFrom).ToList();
            }
            if(model.DateFrom == null && model.DateTo != null)
            {
                var dateTo = DateTime.Parse(model.DateTo);
                lstInvoice = lstInvoice.Where(x => x.PurchaseDate <= dateTo).ToList();
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
        [HttpPost]
        public JsonResult DeleteInvoiceChecked(List<int> id)
        {
            Invoice entity = new Invoice();
            foreach(var item in id)
            {
                entity = data.Invoices.Find(item);
                entity.IsDeleted = true;
                data.SaveChanges();
            }
            return ReturnInvoice();
        }
        public JsonResult DeleteInvoice(int id)
        {
            Invoice entity = new Invoice();
            entity = data.Invoices.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return ReturnInvoice();
        }

        public JsonResult ReturnViewInvoiceById(int id)
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

        public JsonResult ReturnViewDetailInvoiceById(int id)
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

        public JsonResult Confirm(InvoiceManagerViewModel model)
        {
            var NotEnoughStock = false;
            Invoice entity = new Invoice();
            Product Productentity = new Product();

            var prdList = new List<InvoiceDetailMangerViewModel>();
            prdList = (from a in data.Invoice_Detail
                       join b in data.Products on a.Product_id equals b.id
                       where a.IsDeleted == false && a.Invoice_id == model.Id
                       select new InvoiceDetailMangerViewModel
                       {
                           Product_id = a.Product_id,
                           Amount = a.Amount,
                       }).ToList();
            foreach (var item in prdList)
            {
                Productentity = data.Products.Find(item.Product_id);
                if (Productentity.Amount < item.Amount)
                {
                    NotEnoughStock = true; break;
                }
            }
            if (NotEnoughStock == false)
            {
                foreach (var item in prdList)
                {
                    Productentity = data.Products.Find(item.Product_id);
                    Productentity.Amount = Productentity.Amount - item.Amount;
                    data.SaveChanges();
                }
            }
            if (NotEnoughStock == true)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                entity = data.Invoices.Find(model.Id);
                entity.Status = model.Status;
                data.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateInvoice(InvoiceManagerViewModel model)
        {
            Invoice entity = new Invoice();
            entity = data.Invoices.Find(model.Id);
            entity.DeliveryAddress = model.DeliveryAddress;
            entity.DeliveryPhoneNum = model.DeliveryPhoneNum;
            entity.CustomerName = model.CustomerName;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Seen(InvoiceManagerViewModel model)
        {
            Invoice entity = new Invoice();
            entity = data.Invoices.Find(model.Id);
            entity.Status = model.Status;
            data.SaveChanges();
            return Json(new { Status = "Chưa hoàn thành" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintViewToPdf(int id)
        {
            var report = new ActionAsPdf("InvoiceOutPdf",new { id=id});
            return report;
        }
        public ActionResult InvoiceOutPdf(int id)
        {
            List<InvoiceOutPdfViewModel> invoicePdf = new List<InvoiceOutPdfViewModel>();
            invoicePdf = (from a in data.ProducTypes
                          join b in data.Products on a.Id equals b.ProductType_id
                          join c in data.Invoice_Detail on b.id equals c.Product_id
                          join d in data.Invoices on c.Invoice_id equals d.Id
                          where d.Id == id && c.IsDeleted == false && d.IsDeleted==false
                          select new InvoiceOutPdfViewModel
                          {
                              Id = a.Id,
                              ProductName = b.ProductName,
                              ProductTypeName = a.TypeName,
                              CustomerName = d.CustomerName,
                              Price = b.Price,
                              PhoneName = d.DeliveryPhoneNum,
                              Address = d.DeliveryAddress,
                              MonthsWarranty = b.MonthWarranty,
                              IsDeleted = c.IsDeleted,
                              CreateBy = c.CreateBy,
                              CreateDate = c.CreateDate,
                              Amount = c.Amount
                          }).ToList();
            return View(invoicePdf);
        }

        public JsonResult Notification()
        {
            var model = new List<InvoiceManagerViewModel>();
            model = data.Invoices.Where(x => x.IsDeleted == false && x.Status == "Đang chờ xem").Select(x => new InvoiceManagerViewModel
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
    }
}