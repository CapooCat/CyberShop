using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberShop.Common;
using CyberShop.Areas.Admin.Models;
using Rotativa;
using Data;
namespace CyberShop.Areas.Admin.Controllers
{
    public class InvoiceOutManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/InvoiceOutManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnInvoiceDetailSession()
        {
            var sessionInvoiceDetail = Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION];
            List<InvoiceOutManagerViewModel> model = (List<InvoiceOutManagerViewModel>)sessionInvoiceDetail;
            List<object> ReturnData = new List<object>();
            if (model != null)
            {
                foreach (InvoiceOutManagerViewModel item in model)
                {
                    ReturnData.Add(new InvoiceOutManagerViewModel
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

        public JsonResult AddProductToInvoiceOut(int id)
        {
            var sessionInvoiceDetail = Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION];
            var product = new List<ProductManagerViewModel>();
            product = (from a in data.ProducTypes
                   join b in data.Products on a.Id equals b.ProductType_id
                   join c in data.Brands on b.Brand_id equals c.Id
                   where b.id == id
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
            if (sessionInvoiceDetail != null)
            {
                var list = (List<InvoiceOutManagerViewModel>)sessionInvoiceDetail;
                if (list.Exists(x => x.Product_Id == id))
                {
                    foreach (var item in list)
                    {
                        if (item.Product_Id == id)
                        {
                            item.Amount += 1;
                        }
                    }
                    Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION] = list;
                }
                else
                {
                    var item = new InvoiceOutManagerViewModel();
                    item.Product_Id = id;
                    item.ProductName = data.Products.Where(x => x.id == id).Select(x => x.ProductName).FirstOrDefault();
                    item.BrandName = product.First().BrandName;
                    item.ProductTypeName = product.First().ProductTypeName;
                    item.Amount = 1;
                    item.Price = product.First().Price;
                    item.WarrantyMonths = product.First().MonthWarranty;
                    item.BrandName = product.First().BrandName;
                    list.Add(item);
                    Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION] = list;
                }
            }
            else
            {
                var item = new InvoiceOutManagerViewModel();
                var listInvoiceDetail = new List<InvoiceOutManagerViewModel>();
                item.Product_Id = product.First().id;
                item.ProductName = product.First().ProductName;
                item.BrandName = product.First().BrandName;
                item.ProductTypeName = product.First().ProductTypeName;
                item.WarrantyMonths = product.First().MonthWarranty;
                item.BrandName = product.First().BrandName;
                item.Price = product.First().Price;
                item.Amount = 1;
                listInvoiceDetail.Add(item);
                Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION] = listInvoiceDetail;
            }
            return ReturnInvoiceDetailSession();
        }
        public JsonResult RemoveItem(int id)
        {
            var invoiceOut = Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION];
            List<InvoiceOutManagerViewModel> invoiceOutList = (List<InvoiceOutManagerViewModel>)invoiceOut;
            foreach (InvoiceOutManagerViewModel item in invoiceOutList.ToList())
            {
                if (item.Product_Id == id)
                {
                    invoiceOutList.Remove(item);
                }
            }
            Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION] = invoiceOutList;
            return ReturnInvoiceDetailSession();
        }
        public JsonResult DeleteInvoiceOutChecked(List<int> id)
        {
            var invoiceOut = Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION];
            List<InvoiceOutManagerViewModel> invoiceOutList = (List<InvoiceOutManagerViewModel>)invoiceOut;
            invoiceOutList.RemoveAll(r => id.Any(a => a == r.Product_Id));
            Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION] = invoiceOutList;
            return ReturnInvoiceDetailSession();
        }
        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("InvoiceOutPdf");
            return report;
        }
        public ActionResult InvoiceOutPdf()
        {
            var newInvoiceId = data.Invoices.OrderByDescending(x => x.Id).First().Id;
            List<InvoiceOutPdfViewModel> invoicePdf = new List<InvoiceOutPdfViewModel>();
            invoicePdf = (from a in data.ProducTypes
                                   join b in data.Products on a.Id equals b.ProductType_id
                                   join c in data.Invoice_Detail on b.id equals c.Product_id
                                   join d in data.Invoices on c.Invoice_id equals d.Id
                                   where d.Id == newInvoiceId && d.IsDeleted==false
                                   select new InvoiceOutPdfViewModel
                                   {
                                       Id = a.Id,
                                       ProductName = b.ProductName,
                                       ProductTypeName = a.TypeName,
                                       CustomerName = d.CustomerName,
                                       Price = b.Price,
                                       PhoneName = d.DeliveryPhoneNum,
                                       Address= d.DeliveryAddress,
                                       MonthsWarranty= b.MonthWarranty,
                                       IsDeleted = c.IsDeleted,
                                       CreateBy = c.CreateBy,
                                       CreateDate = c.CreateDate,
                                       Amount = c.Amount
                                   }).ToList();
            return View(invoicePdf);
        }
        [HttpPost]
        public JsonResult SubmitInvoiceOut(InvoiceOutManagerViewModel model)
        {
            Invoice invoice = new Invoice();
            List<InvoiceOutManagerViewModel> invoiceOutList = (List<InvoiceOutManagerViewModel>)Session[Common.CommonConstantUser.INVOICEDETAIL_SESSION];
            double? total = 0;
            foreach (InvoiceOutManagerViewModel item in invoiceOutList)
            {
                total += item.Price * item.Amount;
            }

            var CheckAccount = new List<CustomerManagerViewModel>();
            CheckAccount = data.Users.Where(x => x.IsDeleted == false && x.PhoneNum == model.NumberPhone).Select(x => new CustomerManagerViewModel
            {
                id = x.id
            }).ToList();

            if (CheckAccount.Count > 0)
            {
                foreach (var item in CheckAccount)
                {
                    invoice.User_id = item.id;
                }
            } else
            {
                invoice.User_id = null;
            }
            invoice.CustomerName = model.CustomerName;
            invoice.DeliveryAddress = model.Address;
            invoice.DeliveryPhoneNum = model.NumberPhone;
            invoice.IsDeleted = false;
            invoice.Status = "Chưa hoàn thành";
            invoice.CreateBy = "Admin";
            invoice.CreateDate = DateTime.Now;
            invoice.Total = total;
            invoice.PurchaseDate = DateTime.Now;
            var invoiceDao = new InvoiceDao().InsertInvoice(invoice);
            //Tạo chi tiết hóa đơn
            Invoice_Detail inDetail = new Invoice_Detail();
            Product product = new Product();
            foreach (InvoiceOutManagerViewModel item in invoiceOutList)
            {
                inDetail.Invoice_id = data.Invoices.OrderByDescending(x => x.Id).First().Id;
                inDetail.Product_id = item.Product_Id.Value;
                inDetail.Amount = item.Amount;
                inDetail.Price = item.Price;
                var warranty = Convert.ToInt32(data.Products.First(x => x.id == item.Product_Id).MonthWarranty);
                inDetail.WarrantyExpires = DateTime.Now.AddMonths(warranty);
                inDetail.CreateBy = "Admin";
                inDetail.CreateDate = DateTime.Now;
                inDetail.IsDeleted = false;
                var inDetailDao = new InvoiceDetailDao().InsertInvoiceDetail(inDetail);

                product = data.Products.Find(item.Product_Id);
                data.SaveChanges();
            }
            invoiceOutList.RemoveAll(x => invoiceOutList.Any());
            return Json(new { success=true}, JsonRequestBehavior.AllowGet);
        }
    }
}