using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
using System.IO;
namespace CyberShop.Areas.Admin.Controllers
{
    public class BuildManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/ProductManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReturnPC()
        {
            var model = new List<ProductManagerViewModel>();
            model = (from a in data.Products
                     where a.IsDeleted == false && a.ProductType_id == 20
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

    }
}