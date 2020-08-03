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
            if(userDao.KTTaiKhoan(model.Username) && userDao.KTMatKhau(Encryptor.MD5Hash(model.Password)) && userDao.IsAdmin(model.Username))
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
    }
}