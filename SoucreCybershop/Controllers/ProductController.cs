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
            model = (from a in data.Categoryies
                     where a.Metatitle == metatitle
                     join b in data.ProducTypes on metatitle equals b.Metatitle
                     select new CategoryViewModel
                     {
                         CateName = a.CategoryName,
                         breadcrumb = b.TypeName
                     }).ToList();
            return View("SpTheoDanhMuc", model);
        }

        public JsonResult CategoryJSON(string metatitle)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.Categoryies
                     join b in data.ProducTypes on a.Id equals b.Category_id
                     join c in data.Products on b.Id equals c.ProductType_id
                     where a.Metatitle == metatitle
                     select new ProductCategoryViewModel
                     {
                         id = c.id,
                         Brand_id = c.Brand_id,
                         Promotion_id = c.Promotion_id,
                         ProductName = c.ProductName,
                         Info = c.Info,
                         Price = c.Price,
                         MonthWarranty = c.MonthWarranty,
                         Image = c.Image,
                         IsDeleted = c.IsDeleted,
                         CreateBy = c.CreateBy,
                         CreateDate = c.CreateDate,
                         ModifiedBy = c.ModifiedBy,
                         ModifiedDate = c.ModifiedDate,
                         ProductType_id = c.ProductType_id
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
                    ProductType_id = item.ProductType_id
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
        public ActionResult CategoryDetail(string metatitle)
        {
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            model = (from a in data.CategoryDetails
                     where a.MetaTitle == metatitle
                     join b in data.ProducTypes on metatitle equals b.Metatitle
                     select new CategoryViewModel
                     {
                         CateName = a.CateDetailName,
                         breadcrumb = b.TypeName
                     }).ToList();
            return View("SpTheoDanhMuc", model);
        }

        public JsonResult CategoryDetailJSON(string metatitle)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where a.Metatitle == metatitle
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         Promotion_id = b.Promotion_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ModifiedBy = b.ModifiedBy,
                         ModifiedDate = b.ModifiedDate,
                         ProductType_id = b.ProductType_id
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
                    ProductType_id = item.ProductType_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CategoryDetailSuggest(string metatitle)
        {
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            model = (from a in data.CategoryDetailSuggests
                     where a.Metatitle == metatitle
                     join b in data.CategoryDetails on a.CategoryDetail_id equals b.Id
                     join c in data.ProducTypes on b.MetaTitle equals c.Metatitle
                     select new CategoryViewModel
                     {
                         CateName = a.SuggestName,
                         breadcrumb = c.TypeName
                     }).ToList();
            return View("SpTheoDanhMuc", model);
        }

        public JsonResult CategoryDetailSuggestJSON(string metatitle, string brand)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.ProducTypes
                     join b in data.Products on a.Id equals b.ProductType_id
                     where b.MetaTitle.Contains(metatitle)
                     select new ProductCategoryViewModel
                     {
                         id = b.id,
                         Brand_id = b.Brand_id,
                         Promotion_id = b.Promotion_id,
                         ProductName = b.ProductName,
                         Info = b.Info,
                         Price = b.Price,
                         MonthWarranty = b.MonthWarranty,
                         Image = b.Image,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         ModifiedBy = b.ModifiedBy,
                         ModifiedDate = b.ModifiedDate,
                         ProductType_id = b.ProductType_id
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
                    ProductType_id = item.ProductType_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SanPham()
        {
            var productList = data.Products.ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in productList)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    Promotion_id = item.Promotion_id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price =item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ProductType_id = item.ProductType_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

    }
}