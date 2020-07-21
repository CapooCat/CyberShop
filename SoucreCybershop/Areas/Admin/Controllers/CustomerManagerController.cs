using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
namespace CyberShop.Areas.Admin.Controllers
{
    public class CustomerManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/CustomerManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnCustomer()
        {
            var model = new List<CustomerManagerViewModel>();
            model = data.Users.Where(x=>x.IsDeleted==false && x.UserType!="admin").Select(x => new CustomerManagerViewModel
            {
                id = x.id,
                Username = x.Username,
                UserType = x.UserType,
                Name = x.Name,
                Position = x.Position,
                Address = x.Address,
                PhoneNum = x.PhoneNum,
                Email = x.Email,
                IsDeleted = x.IsDeleted,
                CreateDate=x.CreateDate
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new CustomerManagerViewModel
                {
                    id = item.id,
                    Username = item.Username,
                    UserType = item.UserType,
                    Name = item.Name,
                    Position = item.Position,
                    Address = item.Address,
                    PhoneNum = item.PhoneNum,
                    Email = item.Email,
                    IsDeleted = item.IsDeleted,
                    CreateDate = item.CreateDate
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilterUser(CustomerManagerViewModel model)
        {
            var lstUser = new List<CustomerManagerViewModel>();
            lstUser = data.Users.Where(x =>x.IsDeleted==false).Select(x => new CustomerManagerViewModel
            {
                id = x.id,
                Username = x.Username,
                UserType = x.UserType,
                Name = x.Name,
                Position = x.Position,
                Address = x.Address,
                PhoneNum = x.PhoneNum,
                Email = x.Email,
                IsDeleted = x.IsDeleted,
                CreateDate = x.CreateDate
            }).ToList();
            lstUser = lstUser.Where(x => ((model.Name == null) || (x.Name.Contains(model.Name))))
                                      .Where(x => ((model.PhoneNum == null) || (x.PhoneNum==model.PhoneNum)))
                                      .Where(x => ((model.Email == null) || (x.Email==model.Email)))
                                      .ToList();

            if (model.CreatedDate != null)
            {
                var createdDate = DateTime.Parse(model.CreatedDate);
                lstUser = lstUser.Where(x => x.CreateDate == createdDate).ToList();
            }
            List<object> ReturnData = new List<object>();
            foreach (var item in lstUser)
            {
                ReturnData.Add(new CustomerManagerViewModel
                {
                    id = item.id,
                    Username = item.Username,
                    UserType = item.UserType,
                    Name = item.Name,
                    Position = item.Position,
                    Address = item.Address,
                    PhoneNum = item.PhoneNum,
                    Email = item.Email,
                    IsDeleted = item.IsDeleted,
                    CreateDate = item.CreateDate
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}