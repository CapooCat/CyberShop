using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberShop.Models;
using Data;
namespace CyberShop.Controllers
{
    public class BuildController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: BuildPC
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnPCPart(int id)
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            model = data.Products.Where(x => x.ProductType_id == id).Select(x => new ProductViewModel
            {
                id = x.id,
                ProductName = x.ProductName,
                Brand_id = x.Brand_id,
                ProductType_id = x.ProductType_id,
                Price = x.Price,
                Amount = x.Amount,
                Image = x.Image,
                MetaTitle = x.MetaTitle,
                Info = x.Info,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                IsDeleted = x.IsDeleted,
                MonthWarranty = x.MonthWarranty,
                SalePercent = x.SalePercent
            }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    Price = item.Price,
                    Amount = item.Amount,
                    Image = item.Image,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    IsDeleted = item.IsDeleted,
                    MonthWarranty = item.MonthWarranty,
                    SalePercent = item.SalePercent
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}