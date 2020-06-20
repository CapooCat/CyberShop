using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CyberShop.Areas.Admin.Controllers
{
    public class CustomerManagerController : Controller
    {
        // GET: Admin/CustomerManager
        public ActionResult Index()
        {
            return View();
        }
    }
}