using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
namespace CyberShop.Areas.Admin.Controllers
{
    public class ProductManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/ProductManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult ReturnProduct()
        {
            var model = new List<ProductManagerViewModel>();
            model = (from a in data.Products
                     join b in data.ProducTypes on a.ProductType_id equals b.Id
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         ProductTypeName=b.TypeName,
                         ProductName = a.ProductName,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         Price = a.Price,
                         SalePercent = a.SalePercent,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         Amount = a.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    ProductTypeName = item.ProductTypeName,
                    ProductName = item.ProductName,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    Price = item.Price,
                    SalePercent = item.SalePercent,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}