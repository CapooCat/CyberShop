using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Models;
namespace CyberShop.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        public ActionResult Index()
        {
            var model = new CustomerOrderViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(CustomerOrderViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}