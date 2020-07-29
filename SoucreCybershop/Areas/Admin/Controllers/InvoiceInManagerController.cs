using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;

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
                    IsDeleted =item.IsDeleted,
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
            } else
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
                Productentity.Amount += Productentity.Amount + item.Amount;
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
    }
}