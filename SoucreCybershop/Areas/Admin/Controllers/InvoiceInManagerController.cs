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

        public JsonResult ReturnInvoiceIn()
        {
            var model = new List<InvoiceInManagerViewModel>();
            model = data.InOrders.Where(x => x.IsDeleted == false).Select(x => new InvoiceInManagerViewModel
            {
                Id = x.Id,
                Status = x.Status,
                Total = x.Total,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceInManagerViewModel
                {
                    Id = item.Id,
                    Status = item.Status,
                    Total = item.Total,
                    IsDeleted =item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}