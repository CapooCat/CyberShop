using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Models;
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
        public ActionResult TatCaVGA()
        {
            List<Product> model = new List<Product>();
            model = data.Products.Where(x=>x.ProductType_id==1).ToList();
            return View("SpTheoDanhMuc",model);
        }
        public ActionResult Category(string metatitle)
        {
            List<ProductCategoryViewModel> model = new List<ProductCategoryViewModel>();
            model = (from a in data.Categoryies
                     join b in data.ProducTypes on a.Id equals b.Category_id
                     join c in data.Products on b.Id equals c.ProductType_id
                     where a.Metatitle == metatitle
                     select new ProductCategoryViewModel
                     {
                         id=c.id,
                         Brand_id=c.Brand_id,
                         Promotion_id=c.Promotion_id,
                         ProductName=c.ProductName,
                         Info=c.Info,
                         Price=c.Price,
                         MonthWarranty=c.MonthWarranty,
                         Image=c.Image,
                         IsDeleted=c.IsDeleted,
                         CreateBy=c.CreateBy,
                         CreateDate=c.CreateDate,
                         ModifiedBy=c.ModifiedBy,
                         ModifiedDate=c.ModifiedDate,
                         ProductType_id=c.ProductType_id
                     }).ToList();
            return View("SpTheoDanhMuc", model);
        }
        public ActionResult CategoryDetail(string metatitle)
        {
            List<ProductCategoryViewModel> model = new List<ProductCategoryViewModel>();
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
            return View("SpTheoDanhMuc", model);
        }

    }
}