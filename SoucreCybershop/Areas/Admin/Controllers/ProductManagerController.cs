using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CyberShop.Areas.Admin.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: Admin/ProductManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}