using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CyberShop.Areas.Admin.Controllers
{
    public class InvoiceOutManagerController : Controller
    {
        // GET: Admin/InvoiceOutManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}