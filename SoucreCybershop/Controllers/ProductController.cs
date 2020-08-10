using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CyberShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        ProductDao productDao = new ProductDao();
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public ActionResult Index()
        {
            List<Product> model = new List<Product>();
            model = productDao.ListProduct();
            return View(model);
        }
        public ActionResult SpTheoDanhMuc(List<ProductCategoryViewModel> model)
        {
            return View(model);
        }
        public ActionResult Category(string metatitle)
        {
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            model = (from a in data.Categories
                     from b in data.ProducTypes
                     where a.Metatitle == metatitle
                     select new CategoryViewModel
                     {
                         CateName = b.TypeName,
                         breadcrumb = a.Name
                     }).ToList();
            return View("SpTheoDanhMuc", model);
        }

        public ActionResult Search(string Value)
        {
            if (Value != null)
            {
                Value = System.Uri.UnescapeDataString(Value);
            }
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            model.Add(
                     new CategoryViewModel
                     {
                         CateName = "Tìm kiếm \"" + Value + "\"",
                         breadcrumb = "Tìm kiếm"
                     });
            return View("SpTheoDanhMuc", model);
        }

        public JsonResult SearchJSON(string Value)
        {
            Value = System.Uri.UnescapeDataString(Value);
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.Products
                     where a.ProductName.Contains(Value) && a.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = a.id,
                         Brand_id = a.Brand_id,
                         ProductName = a.ProductName,
                         Info = a.Info,
                         Price = a.Price,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         ProductType_id = a.ProductType_id,
                         Amount = a.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryJSON(string metatitle)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.Categories
                     join b in data.ProducTypes on a.Id equals b.Category_id
                     join c in data.Products on b.Id equals c.ProductType_id
                     where a.Metatitle == metatitle && c.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = c.id,
                         Brand_id = c.Brand_id,
                         ProductName = c.ProductName,
                         Info = c.Info,
                         Price = c.Price,
                         MonthWarranty = c.MonthWarranty,
                         Image = c.Image,
                         IsDeleted = c.IsDeleted,
                         CreateBy = c.CreateBy,
                         CreateDate = c.CreateDate,
                         ProductType_id = c.ProductType_id,
                         Amount = c.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Category()
        //{
        //    List<ProductCategoryViewModel> model = new List<ProductCategoryViewModel>();
        //    return View("SpTheoDanhMuc", model);
        //}
        //public JsonResult ReturnTest()
        //{
        //    return Json(new { data="ok"}, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult CategoryDetailJSON(string metatitle)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where a.Metatitle == metatitle && b.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ProductType_id = b.ProductType_id,
                         Amount = b.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    Promotion_id = item.Promotion_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryDetailSuggestJSON(string metatitle, string brand)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where b.MetaTitle.Contains(metatitle) && b.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ProductType_id = b.ProductType_id,
                         Amount = b.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SortPriceLessThan(string type, string number)
        {
            var model = new List<ProductCategoryViewModel>();
            int PriceToSort = Convert.ToInt32(number + "000000");
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where b.MetaTitle.Contains(type) && b.Price < PriceToSort && b.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ProductType_id = b.ProductType_id,
                         Amount = b.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SortPriceMoreThan(string type, string number)
        {
            var model = new List<ProductCategoryViewModel>();
            int PriceToSort = Convert.ToInt32(number + "000000");
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where b.MetaTitle.Contains(type) && b.Price > PriceToSort && b.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ProductType_id = b.ProductType_id,
                         Amount = b.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SortPriceFromTo(string type, string NumberFrom, string NumberTo)
        {
            var model = new List<ProductCategoryViewModel>();
            int PriceToSortFrom = Convert.ToInt32(NumberFrom + "000000");
            int PriceToSortTo = Convert.ToInt32(NumberTo + "000000");
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where b.MetaTitle.Contains(type) && b.Price > PriceToSortFrom && b.Price < PriceToSortTo && b.IsDeleted == false
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ProductType_id = b.ProductType_id,
                         Amount = b.Amount
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SanPham()
        {
            var productList = data.Products.Where(x => x.IsDeleted == false).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in productList)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price =item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ProductType_id = item.ProductType_id,
                    Amount = item.Amount
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CurrentUrl()
        {
            return Json(Session[Common.CommonConstantUser.URL_REFERRER], JsonRequestBehavior.AllowGet);
        } 

    }
}