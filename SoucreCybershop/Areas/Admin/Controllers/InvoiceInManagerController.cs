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
    public class InvoiceInManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/InvoiceInManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReturnInvoiceIn()
        {
            var model = new List<InvoiceInManagerViewModel>();
            model = data.InOrders.Where(x => x.IsDeleted == false).Select(x => new InvoiceInManagerViewModel
            {
                Id = x.Id,
                Status = x.Status,
                Total = x.Total,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceInManagerViewModel
                {
                    Id = item.Id,
                    Status = item.Status,
                    Total = item.Total,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FilterInvoiceIn(InvoiceInManagerViewModel model)
        {
            var lstInvoice = new List<InvoiceInManagerViewModel>();
            if (model.Product_id != null)
            {
                lstInvoice = (from a in data.InOrder_Detail
                              join c in data.InOrders on a.InOrder_id equals c.Id
                              where a.IsDeleted == false && a.Product_id == model.Product_id
                              select new InvoiceInManagerViewModel
                              {
                                  Id = c.Id,
                                  Status = c.Status,
                                  Total = c.Total,
                                  IsDeleted = c.IsDeleted,
                                  CreateBy = c.CreateBy,
                                  CreateDate = c.CreateDate,
                              }).ToList();
            }
            else
            {
                lstInvoice = data.InOrders.Where(x => ((model.Id == null) || (x.Id == model.Id)))
                .Select(x => new InvoiceInManagerViewModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    Total = x.Total,
                    IsDeleted = x.IsDeleted,
                    CreateBy = x.CreateBy,
                    CreateDate = x.CreateDate
                }).ToList();
            }
            if (model.DateFrom != null && model.DateTo == null)
            {
                var dateFrom = DateTime.Parse(model.DateFrom);
                lstInvoice = lstInvoice.Where(x => x.CreateDate == dateFrom).ToList();
            }
            if (model.DateFrom == null && model.DateTo != null)
            {
                var dateTo = DateTime.Parse(model.DateTo);
                lstInvoice = lstInvoice.Where(x => x.CreateDate == dateTo).ToList();
            }
            if (model.DateFrom != null && model.DateTo != null)
            {
                var dateFrom = DateTime.Parse(model.DateFrom);
                var dateTo = DateTime.Parse(model.DateTo);
                lstInvoice = lstInvoice.Where(x => x.CreateDate >= dateFrom && x.CreateDate <= dateTo).ToList();
            }
            lstInvoice = lstInvoice.Where(x => x.IsDeleted == false).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in lstInvoice)
            {
                ReturnData.Add(new InvoiceInManagerViewModel
                {
                    Id = item.Id,
                    Status = item.Status,
                    Total = item.Total,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnDetailInvoiceInById(int id)
        {
            var model = new List<InvoiceInDetailManagerViewModel>();
            model = (from a in data.InOrder_Detail
                     join b in data.Products on a.Product_id equals b.id
                     join c in data.InOrders on a.InOrder_id equals c.Id
                     where a.IsDeleted == false && a.InOrder_id == id
                     select new InvoiceInDetailManagerViewModel
                     {
                         Id = a.Id,
                         InOrder_id = a.InOrder_id,
                         Product_id = a.Product_id,
                         ProductName = b.ProductName,
                         Amount = a.Amount,
                         Price = a.Price,
                         IsDeleted = a.IsDeleted,
                         CreateBy = c.CreateBy,
                         CreateDate = a.CreateDate
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceInDetailManagerViewModel
                {
                    Id = item.Id,
                    InOrder_id = item.InOrder_id,
                    Product_id = item.Product_id,
                    ProductName = item.ProductName,
                    Amount = item.Amount,
                    Price = item.Price,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddProductToInvoiceIn(InvoiceInDetailManagerViewModel model)
        {
            var InOrderDetailDao = new InOrderDetailDao();
            var InOrderDetail = new InOrder_Detail();
            var list = new List<InvoiceInDetailManagerViewModel>();

            list = (from a in data.InOrder_Detail
                    where a.InOrder_id == model.InOrder_id && a.Product_id == model.Product_id
                    select new InvoiceInDetailManagerViewModel
                    {
                        Id = a.Id,
                        InOrder_id = a.InOrder_id,
                        Product_id = a.Product_id,
                        IsDeleted = a.IsDeleted
                    }).ToList();

            if (list.Any())
            {
                var n = list.First().Id;
                InOrder_Detail entity = new InOrder_Detail();
                if (list.First().IsDeleted == true)
                {
                    entity = data.InOrder_Detail.Find(n);
                    entity.Amount = model.Amount.GetValueOrDefault();
                    entity.Price = Convert.ToDouble(model.Price);
                    entity.IsDeleted = false;
                    data.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    entity = data.InOrder_Detail.Find(n);
                    entity.Amount += model.Amount.GetValueOrDefault();
                    entity.Price = Convert.ToDouble(model.Price);
                    data.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            else
            {
                InOrderDetail.InOrder_id = model.InOrder_id;
                InOrderDetail.Product_id = model.Product_id;
                InOrderDetail.Price = Convert.ToDouble(model.Price);
                InOrderDetail.Amount = model.Amount.GetValueOrDefault(); ;
                InOrderDetail.IsDeleted = false;
                InOrderDetail.CreateDate = DateTime.Now;
                if (InOrderDetailDao.InsertInOrderDetail(InOrderDetail))
                {
                    InOrder OrderInEntity = new InOrder();
                    OrderInEntity = data.InOrders.Find(model.InOrder_id);
                    OrderInEntity.Total = CalculateTotal(model.InOrder_id);
                    data.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
            }
        }

        public double CalculateTotal(int? id)
        {
            double Total = 0;
            var model = new List<InvoiceInDetailManagerViewModel>();
            model = (from a in data.InOrder_Detail
                     join b in data.Products on a.Product_id equals b.id
                     join c in data.InOrders on a.InOrder_id equals c.Id
                     where a.IsDeleted == false && a.InOrder_id == id
                     select new InvoiceInDetailManagerViewModel
                     {
                         Amount = a.Amount,
                         Price = a.Price,
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                Total = Total + (Convert.ToDouble(item.Amount) * Convert.ToDouble(item.Price));
            }
            return Total;
        }

        public JsonResult ReturnInvoiceInById(int id)
        {
            var model = new List<InvoiceInManagerViewModel>();
            model = data.InOrders.Where(x => x.IsDeleted == false && x.Id == id).Select(x => new InvoiceInManagerViewModel
            {
                Id = x.Id,
                Status = x.Status,
                Total = x.Total,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceInManagerViewModel
                {
                    Id = item.Id,
                    Status = item.Status,
                    Total = item.Total,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDetailInvoiceIn(int id)
        {
            InOrder_Detail entity = new InOrder_Detail();
            entity = data.InOrder_Detail.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Confirm(InvoiceManagerViewModel model)
        {
            InOrder entity = new InOrder();
            entity = data.InOrders.Find(model.Id);
            entity.Status = model.Status;
            data.SaveChanges();
            Product Productentity = new Product();

            var prdList = new List<InvoiceInDetailManagerViewModel>();
            prdList = (from a in data.InOrder_Detail
                       join b in data.Products on a.Product_id equals b.id
                       where a.IsDeleted == false && a.InOrder_id == model.Id
                       select new InvoiceInDetailManagerViewModel
                       {
                           Product_id = a.Product_id,
                           Amount = a.Amount,
                       }).ToList();
            foreach (var item in prdList)
            {
                Productentity = data.Products.Find(item.Product_id);
                Productentity.Amount = Productentity.Amount + item.Amount;
                data.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteInvoiceIn(int id)
        {
            InOrder entity = new InOrder();
            entity = data.InOrders.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return ReturnInvoiceIn();
        }

        public JsonResult ReturnInvoiceInSession()
        {
            var sessionInvoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            List<InvoiceInCreateViewModel> model = (List<InvoiceInCreateViewModel>)sessionInvoiceIn;
            List<object> ReturnData = new List<object>();
            if (model != null)
            {
                foreach (InvoiceInCreateViewModel item in model)
                {
                    ReturnData.Add(new InvoiceInCreateViewModel
                    {
                        Product_Id = item.Product_Id,
                        ProductName = item.ProductName,
                        BrandName = item.BrandName,
                        ProductTypeName = item.ProductTypeName,
                        WarrantyMonths = item.WarrantyMonths,
                        Price = item.Price,
                        Amount = item.Amount
                    });
                }
                return Json(ReturnData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult TestExistInvoiceIn(int id)
        {
            var sessionInvoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            List<InvoiceInCreateViewModel> model = (List<InvoiceInCreateViewModel>)sessionInvoiceIn;
            if (model!=null)
            {
                bool exist = model.Exists(x => x.Product_Id == id);
                if (exist)
                {
                    return Json(new { success = true}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddProductToCreateInvoiceIn(int Product_id, double Price,int Amount)
        {
            var sessionInvoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            var product = new List<ProductManagerViewModel>();
            product = (from a in data.ProducTypes
                       join b in data.Products on a.Id equals b.ProductType_id
                       join c in data.Brands on b.Brand_id equals c.Id
                       where b.id == Product_id
                       select new ProductManagerViewModel
                       {
                           id = b.id,
                           Brand_id = b.Brand_id,
                           ProductType_id = b.ProductType_id,
                           ProductTypeName = a.TypeName,
                           ProductName = b.ProductName,
                           MetaTitle = b.MetaTitle,
                           Info = b.Info,
                           Price = b.Price,
                           SalePercent = b.SalePercent,
                           MonthWarranty = b.MonthWarranty,
                           Image = b.Image,
                           IsDeleted = b.IsDeleted,
                           CreateBy = b.CreateBy,
                           CreateDate = b.CreateDate,
                           Amount = b.Amount,
                           BrandName = c.BrandName

                       }).ToList();
            if (sessionInvoiceIn != null)
            {
                var list = (List<InvoiceInCreateViewModel>)sessionInvoiceIn;
                if (list.Exists(x => x.Product_Id == Product_id))
                {
                    foreach (var item in list)
                    {
                        if (item.Product_Id == Product_id)
                        {
                            item.Amount += Amount;
                        }
                    }
                    Session[Common.CommonConstantUser.INVOICEIN_SESSION] = list;
                }
                else
                {
                    var item = new InvoiceInCreateViewModel();
                    item.Product_Id = Product_id;
                    item.ProductName = data.Products.Where(x => x.id == Product_id).Select(x => x.ProductName).FirstOrDefault();
                    item.BrandName = product.First().BrandName;
                    item.ProductTypeName = product.First().ProductTypeName;
                    item.Amount = Amount;
                    item.Price = Price;
                    item.WarrantyMonths = product.First().MonthWarranty;
                    item.BrandName = product.First().BrandName;
                    list.Add(item);
                    Session[Common.CommonConstantUser.INVOICEIN_SESSION] = list;
                }
            }
            else
            {
                var item = new InvoiceInCreateViewModel();
                var listInvoiceIn = new List<InvoiceInCreateViewModel>();
                item.Product_Id = product.First().id;
                item.ProductName = product.First().ProductName;
                item.BrandName = product.First().BrandName;
                item.ProductTypeName = product.First().ProductTypeName;
                item.WarrantyMonths = product.First().MonthWarranty;
                item.BrandName = product.First().BrandName;
                item.Price = Price;
                item.Amount = Amount;
                listInvoiceIn.Add(item);
                Session[Common.CommonConstantUser.INVOICEIN_SESSION] = listInvoiceIn;
            }
            return ReturnInvoiceInSession();
        }
        public JsonResult RemoveItem(int id)
        {
            var invoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            List<InvoiceInCreateViewModel> invoiceInList = (List<InvoiceInCreateViewModel>)invoiceIn;
            foreach (InvoiceInCreateViewModel item in invoiceInList.ToList())
            {
                if (item.Product_Id == id)
                {
                    invoiceInList.Remove(item);
                }
            }
            Session[Common.CommonConstantUser.INVOICEIN_SESSION] = invoiceInList;
            return ReturnInvoiceInSession();
        }
        public JsonResult DeleteInvoiceInChecked(List<int> id)
        {
            var invoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            List<InvoiceInCreateViewModel> invoiceInList = (List<InvoiceInCreateViewModel>)invoiceIn;
            invoiceInList.RemoveAll(r => id.Any(a => a == r.Product_Id));
            Session[Common.CommonConstantUser.INVOICEIN_SESSION] = invoiceInList;
            return ReturnInvoiceInSession();
        }
        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("InvoiceInPdf");
            return report;
        }
        public ActionResult InvoiceInPdf()
        {
            var newInvoiceId = data.Invoices.OrderByDescending(x => x.Id).First().Id;
            List<InvoiceInPdfViewModel> invoicePdf = new List<InvoiceInPdfViewModel>();
            invoicePdf = (from a in data.ProducTypes
                          join b in data.Products on a.Id equals b.ProductType_id
                          join c in data.Invoice_Detail on b.id equals c.Product_id
                          join d in data.Invoices on c.Invoice_id equals d.Id
                          where d.Id == newInvoiceId && d.IsDeleted == false
                          select new InvoiceInPdfViewModel
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
    }
}