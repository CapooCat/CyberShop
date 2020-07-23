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
        [HttpPost]
        public JsonResult DeleteUserChecked(List<int> id)
        {
            User entity = new User();
            foreach (var item in id)
            {
                entity = data.Users.Find(item);
                entity.IsDeleted = true;
                data.SaveChanges();
            }
            return ReturnCustomer();
        }
        public JsonResult DeleteUser(int id)
        {
            User entity = new User();
            entity = data.Users.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return ReturnCustomer();
        }
        public JsonResult ReturnCustomerById(int id)
        {
            var model = new List<CustomerManagerViewModel>();
            model = data.Users.Where(x => x.IsDeleted == false && x.UserType != "admin" && x.id==id).Select(x => new CustomerManagerViewModel
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
        [HttpPost]
        public JsonResult UpdateUser(CustomerManagerViewModel model)
        {
            User entity = new User();
            entity = data.Users.Find(model.id);
            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.PhoneNum = model.PhoneNum;
            entity.Email = model.Email;
            entity.Username = model.Username;
            if (String.IsNullOrEmpty(model.Password)) { }
            else
            {
                entity.Password = Encryptor.MD5Hash(model.Password);
            }
            data.SaveChanges();
            return ReturnCustomer();
        }
        public JsonResult ReturnDetailInvoiceByUserId(int id)
        {
            var model = new List<InvoiceDetailMangerViewModel>();
            model = (from a in data.Invoices
                     join b in data.Invoice_Detail on a.Id equals b.Invoice_id
                     join c in data.Products on b.Product_id equals c.id
                     where a.IsDeleted == false && a.User_id==id
                     select new InvoiceDetailMangerViewModel
                     {
                         Id = a.Id,
                         Invoice_id = b.Invoice_id,
                         Product_id = b.Product_id,
                         ProductName = c.ProductName,
                         Amount = b.Amount,
                         Price = b.Price,
                         WarrantyExpires = b.WarrantyExpires,
                         IsDeleted = b.IsDeleted,
                         CreateBy = b.CreateBy,
                         CreateDate = b.CreateDate,
                         Status=a.Status
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new InvoiceDetailMangerViewModel
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
                    Status=item.Status
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}