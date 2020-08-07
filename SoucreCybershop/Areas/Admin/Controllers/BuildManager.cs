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

        public JsonResult SavePC(PCDetailManagerViewModel model)
        {
            Detail_PcSets entity = new Detail_PcSets();
            Product product = new Product();
            int ID = data.Detail_PcSets.First(x => x.product_id == model.product_id).id;
            entity = data.Detail_PcSets.Find(ID);
            entity.main_id = model.main_id;
            entity.cpu_id = model.cpu_id;
            entity.ram_id = model.ram_id;
            entity.ssd_id = model.ssd_id;
            entity.hdd_id = model.hdd_id;
            entity.power_id = model.power_id;
            entity.vga_int = model.vga_int;
            entity.case_id = model.case_id;
            entity.monitor_id = model.monitor_id;
            entity.cooler_id = model.cooler_id;
            product = data.Products.Find(model.product_id);
            product.Price = model.Price;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedPC(int id)
        {
            var model = new List<PCDetailManagerViewModel>();
            model = (from a in data.Detail_PcSets
                     where a.IsDeleted == false && a.product_id == id
                     select new PCDetailManagerViewModel
                     {
                         cpu_id = a.cpu_id,
                         main_id = a.main_id,
                         ram_id = a.ram_id,
                         power_id = a.power_id,
                         monitor_id = a.monitor_id,
                         vga_int = a.vga_int,
                         hdd_id = a.hdd_id,
                         ssd_id = a.ssd_id,
                         cooler_id = a.cooler_id,
                         case_id = a.case_id
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new PCDetailManagerViewModel
                {
                    cpu_id = item.cpu_id,
                    main_id = item.main_id,
                    ram_id = item.ram_id,
                    power_id = item.power_id,
                    monitor_id = item.monitor_id,
                    vga_int = item.vga_int,
                    hdd_id = item.hdd_id,
                    ssd_id = item.ssd_id,
                    cooler_id = item.cooler_id,
                    case_id = item.case_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
       

        public JsonResult ReturnPCPart(int id)
        {
            List<ProductManagerViewModel> model = new List<ProductManagerViewModel>();
            model = (from a in data.Products
                     join b in data.ProducTypes on a.ProductType_id equals b.Id
                     where a.ProductType_id == id && a.IsDeleted == false
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName,
                         ProductTypeName = b.TypeName,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         Price = a.Price,
                         Amount = a.Amount,
                         Image = a.Image,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         IsDeleted = a.IsDeleted,
                         MonthWarranty = a.MonthWarranty,
                         SalePercent = a.SalePercent,
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                if (item.Amount > 0)
                {
                    item.CheckStatus = "còn hàng";
                }
                else { item.CheckStatus = "hết hàng"; }
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    ProductTypeName = item.ProductTypeName,
                    Price = item.Price,
                    Amount = item.Amount,
                    Image = item.Image,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    IsDeleted = item.IsDeleted,
                    MonthWarranty = item.MonthWarranty,
                    SalePercent = item.SalePercent,
                    CheckStatus = item.CheckStatus
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnPCItem(int id)
        {
            List<ProductManagerViewModel> model = new List<ProductManagerViewModel>();
            model = (from a in data.Products
                     join b in data.ProducTypes on a.ProductType_id equals b.Id
                     where a.id == id && a.IsDeleted == false
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName,
                         ProductTypeName = b.TypeName,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         Price = a.Price,
                         Amount = a.Amount,
                         Image = a.Image,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         IsDeleted = a.IsDeleted,
                         MonthWarranty = a.MonthWarranty,
                         SalePercent = a.SalePercent
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    ProductTypeName = item.ProductTypeName,
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