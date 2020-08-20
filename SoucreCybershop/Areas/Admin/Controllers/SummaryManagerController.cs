using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
namespace CyberShop.Areas.Admin.Controllers
{
    public class SummaryManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/Summary
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnTurnover()
        {

            var lstInvoice = data.Invoices.Where(x => x.IsDeleted == false && x.Status=="Đã hoàn thành").ToList();
            double? totalTurnover = 0;
            foreach(var item in lstInvoice)
            {
                totalTurnover += item.Total;
            }
            return Json(new { totalTurnover }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnCustomerAmount()
        {
            var lstCustomer = data.Users.Where(x => x.IsDeleted == false && x.UserType!="admin").ToList();
            var totalUser = 0;
            foreach (var item in lstCustomer)
            {
                totalUser += 1;
            }
            return Json(new { totalUser }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnTotalProductSell()
        {

            var lstProductSell = (from a in data.Invoices
                                  join b in data.Invoice_Detail on a.Id equals b.Invoice_id
                                  where a.IsDeleted == false && a.Status == "Đã hoàn thành"
                                  select new {
                                      b.Amount
                                  }).ToList();
            int? totalProductSell = 0;
            foreach (var item in lstProductSell)
            {
                totalProductSell += item.Amount;
            }
            return Json(new { totalProductSell }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnBestSeller()
        {

            var model = new List<BestSellerViewModel>();
            model = (from a in data.Products
                     join b in data.Invoice_Detail on a.id equals b.Product_id
                     join c in data.Invoices on b.Invoice_id equals c.Id
                     where a.IsDeleted == false && c.IsDeleted == false && c.Status=="Đã hoàn thành"
                     group b by new {
                         a.id,
                         a.ProductName,
                         a.Price
                     } into x
                     select new BestSellerViewModel
                     {
                         Product_Id=x.Key.id,
                         ProductName=x.Key.ProductName,
                         Price=x.Key.Price,
                         SellAmount=x.Sum(b=>b.Amount)
                     }).OrderByDescending(x=>x.SellAmount).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new BestSellerViewModel
                {
                    Product_Id = item.Product_Id,
                    ProductName = item.ProductName,
                    Price=item.Price,
                    SellAmount = item.SellAmount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnNewDeal()
        {
            var model = new List<InvoiceManagerViewModel>();
            model = data.Invoices.Where(x => x.IsDeleted == false && x.Status == "Đã hoàn thành").OrderByDescending(x=>x.PurchaseDate).Select(x => new InvoiceManagerViewModel
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
        public JsonResult ReturnTurnoverChart()
        {
            List<object> ReturnData = new List<object>();
            for (int i = 1; i <= 12; i++)
            {
                var model = data.Invoices.Where(x => x.PurchaseDate.Value.Year == 2020 && x.PurchaseDate.Value.Month == i && x.Status=="Đã hoàn thành").ToList();
                if (model != null)
                {
                    double? totalInMonth = 0;
                    foreach (var item in model)
                    {
                        totalInMonth += item.Total;
                    }
                    totalInMonth = totalInMonth / 1000000;
                    ReturnData.Add(totalInMonth);
                }
                else
                {
                    double? totalInMonth = 0;
                    ReturnData.Add(totalInMonth);
                }
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}