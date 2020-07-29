using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;

namespace CyberShop.Areas.Admin.Controllers
{
    public class AddInvoiceInManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/WarrantyManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


    }
}