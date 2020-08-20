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
    public class AddInvoiceInManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/WarrantyManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
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
            if (model != null)
            {
                bool exist = model.Exists(x => x.Product_Id == id);
                if (exist)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
        public JsonResult AddProductToCreateInvoiceIn(int Product_id, double Price, int Amount)
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
            var newInvoiceId = data.InOrders.OrderByDescending(x => x.Id).First().Id;
            List<InvoiceInPdfViewModel> invoicePdf = new List<InvoiceInPdfViewModel>();
            invoicePdf = (from a in data.Brands
                          join b in data.Products on a.Id equals b.ProductType_id
                          join c in data.InOrder_Detail on b.id equals c.Product_id
                          join d in data.ProducTypes on b.ProductType_id equals d.Id
                          join e in data.InOrders on c.InOrder_id equals e.Id
                          where c.InOrder_id == newInvoiceId && c.IsDeleted == false && e.IsDeleted==false
                          select new InvoiceInPdfViewModel
                          {
                              Id = a.Id,
                              ProductName = b.ProductName,
                              BrandName = a.BrandName,
                              ProductTypeName=d.TypeName,
                              Distributor=e.Distributor,
                              Price = c.Price,
                              BrandPhone = a.PhoneNumber,
                              Address = a.Address,
                              IsDeleted = c.IsDeleted,
                              CreateDate = c.CreateDate,
                              Amount = c.Amount
                          }).ToList();
            return View(invoicePdf);
        }
        public JsonResult SubmitInvoiceIn(string distributor)
        {
            InOrder inOder = new InOrder();
            List<InvoiceInCreateViewModel> invoiceInList = (List<InvoiceInCreateViewModel>)Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            double? total = 0;
            foreach (InvoiceInCreateViewModel item in invoiceInList)
            {
                total += item.Price * item.Amount;
            }
            inOder.CreateBy = "Admin";
            inOder.Total = total;
            inOder.IsDeleted = false;
            inOder.Status = "Chưa hoàn thành";
            inOder.CreateDate = DateTime.Now;
            inOder.Distributor = distributor;
            var inorderDao = new InOrderDao().InsertInOrder(inOder);
            //Tạo chi tiết hóa đơn
            InOrder_Detail inOrderDetail = new InOrder_Detail();
            Product product = new Product();
            foreach (InvoiceInCreateViewModel item in invoiceInList)
            {
                inOrderDetail.InOrder_id = data.InOrders.OrderByDescending(x => x.Id).First().Id;
                inOrderDetail.Product_id = item.Product_Id.Value;
                inOrderDetail.Amount = item.Amount;
                inOrderDetail.Price = item.Price;
                var warranty = Convert.ToInt32(data.Products.First(x => x.id == item.Product_Id).MonthWarranty);
                inOrderDetail.CreateDate = DateTime.Now;
                inOrderDetail.IsDeleted = false;
                var inOrderDetailDao = new InOrderDetailDao().InsertInOrderDetail(inOrderDetail);
            }
            invoiceInList.RemoveAll(x => invoiceInList.Any());
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApplyQuantity(int? Id, int Amount)
        {
            var sessionInvoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            if (sessionInvoiceIn != null)
            {
                var list = (List<InvoiceInCreateViewModel>)sessionInvoiceIn;
                if (list.Exists(x => x.Product_Id == Id))
                {
                    foreach (var item in list)
                    {
                        if (item.Product_Id == Id)
                        {
                            item.Amount = Amount;
                        }
                    }
                    Session[Common.CommonConstantUser.INVOICEIN_SESSION] = list;
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Plus(int? Id)
        {
            var sessionInvoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            if (sessionInvoiceIn != null)
            {
                var list = (List<InvoiceInCreateViewModel>)sessionInvoiceIn;
                if (list.Exists(x => x.Product_Id == Id))
                {
                    foreach (var item in list)
                    {
                        if (item.Product_Id == Id)
                        {
                            item.Amount += 1;
                        }
                    }
                    Session[Common.CommonConstantUser.INVOICEIN_SESSION] = list;
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Minus(int? Id)
        {
            var sessionInvoiceIn = Session[Common.CommonConstantUser.INVOICEIN_SESSION];
            if (sessionInvoiceIn != null)
            {
                var list = (List<InvoiceInCreateViewModel>)sessionInvoiceIn;
                if (list.Exists(x => x.Product_Id == Id))
                {
                    foreach (var item in list)
                    {
                        if (item.Product_Id == Id)
                        {
                            if (item.Amount <= 1) { item.Amount = 1; }
                            else
                            {
                                item.Amount -= 1;
                            }
                        }
                    }
                    Session[Common.CommonConstantUser.INVOICEIN_SESSION] = list;
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}