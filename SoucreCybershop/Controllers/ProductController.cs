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
        public ActionResult SpTheoDanhMuc(List<Product> model)
        {
            return View(model);
        }
        public ActionResult TatCaVGA()
        {
            List<Product> model = new List<Product>();
            model = data.Products.Where(x=>x.ProductType_id==1).ToList();
            return View("SpTheoDanhMuc",model);
        }

    }
}