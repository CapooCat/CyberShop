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
                var res = (from a in data.Invoices
                           join b in data.Invoice_Detail on a.Id equals b.Invoice_id
                           join c in data.Products on b.Product_id equals c.id
                           where a.User_id == user.Id
                           select new InvoiceDetailViewModel
                           {
                               Id = b.Id,
                               Invoice_id = a.Id,
                               Product_id = c.id,
                               ProductName = c.ProductName,
                               Amount = b.Amount,
                               Price = b.Price,
                               Status = a.Status,
                               CreateDate = b.CreateDate
                           }).ToList();
                ViewBag.OrderHistory = res;
                return View(model);
            }
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            if (ModelState.IsValid)
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

                var res = (from a in data.Invoices
                           join b in data.Invoice_Detail on a.Id equals b.Invoice_id
                           join c in data.Products on b.Product_id equals c.id
                           where a.User_id == userSession.Id
                           select new InvoiceDetailViewModel
                           {
                               Id = b.Id,
                               Invoice_id = a.Id,
                               Product_id = c.id,
                               ProductName = c.ProductName,
                               Amount = b.Amount,
                               Price = b.Price,
                               Status = a.Status,
                               CreateDate = b.CreateDate
                           }).ToList();
                ViewBag.OrderHistory = res;
            }
            return View(model);
        }
    }
}