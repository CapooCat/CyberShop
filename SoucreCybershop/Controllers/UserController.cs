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
            if (ModelState.IsValid)
            {
                var userDao = new UserDao();
                var userSession = (UserInfo)Session[CommonConstantUser.USER_SESSION];
                var user = new User();
                user.id = userSession.Id;
                user.Name = model.Name;
                user.Address = model.Address;
                user.PhoneNum = model.PhoneNum;
                user.Email = model.Email;
                userDao.UpdateUser(user);
                ViewBag.Success = "Cập nhật thành công";
            }
            return View(model);
        }
        public JsonResult ReturnOrderHistory()
        {
            var userSession = (UserInfo)Session[CommonConstantUser.USER_SESSION];
            if (userSession != null)
            {
                var res = (from a in data.Invoices
                           where a.User_id == userSession.Id && a.IsDeleted == false
                           select new InvoiceViewModel
                           {
                               Id = a.Id,
                               Total = a.Total,
                               Status = a.Status,
                               CreateDate = a.CreateDate
                           }).OrderByDescending(x=>x.CreateDate).ToList();
                List<object> ReturnData = new List<object>();
                foreach (var item in res)
                {
                    ReturnData.Add(new InvoiceViewModel
                    {
                        Id = item.Id,
                        Total = item.Total,
                        Status = item.Status,
                        CreateDate = item.CreateDate
                    });
                }
                return Json(ReturnData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success=false}, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ReturnInvoiceById(int id)
        {
            var model = new List<InvoiceViewModel>();
            model = (from a in data.Invoices
                     where a.IsDeleted == false && a.Id==id
                     select new InvoiceViewModel
                     {
                         Id=a.Id,
                         User_id=a.User_id,
                         CustomerName=a.CustomerName,
                         PurchaseDate=a.PurchaseDate,
                         DeliveryAddress=a.DeliveryAddress,
                         DeliveryPhoneNum = a.DeliveryPhoneNum,
                         Total=a.Total,
                         IsDeleted=a.IsDeleted,
                         CreateBy=a.CreateBy,
                         Status=a.Status
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceViewModel
                {
                    Id = item.Id,
                    User_id = item.User_id,
                    CustomerName = item.CustomerName,
                    PurchaseDate = item.PurchaseDate,
                    DeliveryAddress = item.DeliveryAddress,
                    DeliveryPhoneNum = item.DeliveryPhoneNum,
                    Total = item.Total,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    Status = item.Status
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnInvoiceDetailById(int id)
        {
            var model = new List<InvoiceDetailViewModel>();
            model = (from a in data.Invoices
                     join b in data.Invoice_Detail on a.Id equals b.Invoice_id
                     join c in data.Products on b.Product_id equals c.id
                     where a.IsDeleted == false && a.Id == id && b.IsDeleted==false
                     select new InvoiceDetailViewModel
                     {
                         Id = b.Id,
                         Invoice_id = b.Invoice_id,
                         Product_id = b.Product_id,
                         ProductName = c.ProductName,
                         Amount = b.Amount,
                         Price = b.Price,
                         WarrantyExpires = b.WarrantyExpires,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceDetailViewModel
                {
                    Id = item.Id,
                    Invoice_id = item.Invoice_id,
                    Product_id = item.Product_id,
                    ProductName = item.ProductName,
                    Amount = item.Amount,
                    Price = item.Price,
                    WarrantyExpires = item.WarrantyExpires,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteInvoice(int id)
        {
            Invoice invoice = new Invoice();
            invoice = data.Invoices.Find(id);
            invoice.Status = "Đã hủy";
            data.SaveChanges();
            return Json(new { success=true}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApplyQuantity(int Id, int Amount)
        {
            Invoice_Detail entity = new Invoice_Detail();
            entity = data.Invoice_Detail.Find(Id);
            entity.Amount = Amount;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Plus(int Id)
        {
            Invoice_Detail entity = new Invoice_Detail();
            entity = data.Invoice_Detail.Find(Id);
            entity.Amount += 1;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Minus(int Id)
        {
            Invoice_Detail entity = new Invoice_Detail();
            entity = data.Invoice_Detail.Find(Id);
            if (entity.Amount <= 1) { entity.Amount = 1; }
            else
            {
                entity.Amount -= 1;
            }
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}