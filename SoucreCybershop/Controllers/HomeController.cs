using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
namespace CyberShop.Controllers
{
    public class HomeController : Controller
    {
        ProductDao productDao = new ProductDao();
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public ActionResult Index()
        {
            List<Product> model = new List<Product>();
            model = productDao.ListProduct();
            return View(model);
        }

    }
}