using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Models;
using CyberShop.Common;
namespace CyberShop.Controllers
{
    public class UserController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: User
        public ActionResult Index()
        {
            var model = new UserViewModel();
            if(Session[CommonConstantUser.USER_SESSION]!=null)
            {
                var user = (UserInfo)Session[CommonConstantUser.USER_SESSION];
                model = data.Users.Where(x => x.id == user.Id).Select(x=>new UserViewModel {
                    id=x.id,
                    Name=x.Name,
                    Address=x.Address,
                    Email=x.Email,
                    PhoneNum=x.PhoneNum,
                    Username=x.Username
                }).FirstOrDefault();
                if(model.Address==null || model.PhoneNum==null)
                {
                    ViewBag.Warning = "yes";
                }
                return View(model);
            }
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userDao = new UserDao();
                var userSession = (UserInfo)Session[CommonConstantUser.USER_SESSION];
                var user = new User();
                user.id = userSession.Id;
                user.Name = model.Name;
                user.Address = model.Address;
                user.PhoneNum = model.PhoneNum;
                userDao.UpdateUser(user);
                ViewBag.Success = "Cập nhật thành công";
            }
            return View(model);
        }
    }
}