using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberShop.Areas.Admin.Models;
using Microsoft.Owin.Security;
using Data;
using CyberShop.Common;
using System.Web.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Threading;

namespace CyberShop.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/AdminLogin
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(AdminViewModel model,string returnUrl)
        {
            UserDao userDao = new UserDao();
            if(ModelState.IsValid)
            {
                if (userDao.KTTaiKhoan(model.Username) && userDao.KTMatKhau(Encryptor.MD5Hash(model.Password), model.Username) && userDao.IsAdmin(model.Username))
                {
                    var ident = new ClaimsIdentity(
                    new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
                    new Claim(ClaimTypes.NameIdentifier, model.Username),
                    new Claim(ClaimTypes.Name,data.Users.Where(x=>x.Username==model.Username).First().Name),
                    },
                     DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = false }, ident);
                    var claimsPrincipal = new ClaimsPrincipal(ident);
                    Thread.CurrentPrincipal = claimsPrincipal;
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "CategoryManager");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("Index");
        }
    }
}