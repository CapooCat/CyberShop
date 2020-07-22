using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CyberShop.Areas.Admin.Controllers
{
    public class SummaryManagerController : Controller
    {
        // GET: Admin/Summary
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}