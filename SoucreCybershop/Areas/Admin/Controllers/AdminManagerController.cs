using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
using CyberShop.Common;

namespace CyberShop.Areas.Admin.Controllers
{
    public class AdminManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/WarrantyManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckPassword(AdminViewModel model)
        {
            UserDao userDao = new UserDao();
            if(userDao.KTTaiKhoan(model.Username) && userDao.KTMatKhau(Encryptor.MD5Hash(model.Password), model.Username) && userDao.IsAdmin(model.Username))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            } 
        }

        [HttpPost]
        public JsonResult ChangePassword(AdminViewModel model)
        {
            User entity = new User();
            entity = data.Users.Where(x => x.Username == model.Username).SingleOrDefault();
            if (String.IsNullOrEmpty(model.Password)) { }
            else
            {
                entity.Password = Encryptor.MD5Hash(model.Password);
            }
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeInfo(AdminViewModel model)
        {
            User entity = new User();
            entity = data.Users.Where(x => x.Username == model.Username).SingleOrDefault();
            entity.Email = model.Email;
            entity.PhoneNum = model.PhoneNum;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadInfo(string id)
        {
            var model = new List<AdminViewModel>();
            model = data.Users.Where(x => x.IsDeleted == false && x.Username.Contains(id)).Select(x => new AdminViewModel
            {
                PhoneNum = x.PhoneNum,
                Email = x.Email,
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new AdminViewModel
                {
                    PhoneNum = item.PhoneNum,
                    Email = item.Email,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}