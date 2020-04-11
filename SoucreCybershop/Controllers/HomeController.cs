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
    public class HomeController : Controller
    {
        ProductDao productDao = new ProductDao();
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public ActionResult Index()
        {
            List<Product> model = new List<Product>();
            model = productDao.ListProduct();
            return View(model);
        }
        public ActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            UserDao userDao = new UserDao();
            if(ModelState.IsValid)
            {
                if (userDao.KTTaiKhoan(model.Username))
                {
                    if (userDao.KTMatKhau(Encryptor.MD5Hash(model.Password)))
                    {
                        var user = userDao.getInfo(model.Username);
                        var userSession = new UserInfo();
                        userSession.Id = user.id;
                        userSession.TaiKhoan = user.Username;
                        userSession.Image = user.Image;
                        userSession.HoTen = user.Name;
                        userSession.DateOfBirth = user.DayOfBirth.ToString();
                        Session.Add(CommonConstantUser.USER_SESSION, userSession);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session[CommonConstantUser.USER_SESSION] = null;
            return Redirect("/");
        }
        public ActionResult DetailProduct(int id)
        {
            var model = new Product();
            model= data.Products.Where(x => x.id == id).FirstOrDefault();
            return View(model);
        }


    }
}