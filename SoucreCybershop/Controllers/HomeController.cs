using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Models;
using CyberShop.Common;
using Facebook;
using System.Configuration;
namespace CyberShop.Controllers
{
    public class HomeController : Controller
    {
        ProductDao productDao = new ProductDao();
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public ActionResult Index()
        {
            var spBanChay = new List<ProductHomeViewModel>();
            spBanChay = (from a in data.Products
                     join b in data.Invoice_Detail on a.id equals b.Product_id
                     where a.IsDeleted == false
                     group b by new
                     {
                         a.id,
                         a.ProductName,
                         a.Price,
                         a.Image,
                         a.Amount
                     } into x
                     select new ProductHomeViewModel
                     {
                         id = x.Key.id,
                         ProductName = x.Key.ProductName,
                         oldPrice = x.Key.Price,
                         Image = x.Key.Image,
                         Amount = x.Key.Amount,
                         SellAmount = x.Sum(b => b.Amount)
                     }).OrderByDescending(x => x.SellAmount).Take(12).ToList();
            ViewBag.SpBanchay = spBanChay;

            List<ProductHomeViewModel> spMoi = new List<ProductHomeViewModel>();
            spMoi = data.Products.Where(x => x.IsDeleted == false).OrderByDescending(t => t.CreateDate).Take(12).Select(x=> new ProductHomeViewModel {
                id=x.id,
                ProductName=x.ProductName,
                oldPrice=x.Price,
                Image=x.Image,
                Amount = x.Amount
            }).ToList();
            ViewBag.spMoi = spMoi;
            return View();
        }
        public ActionResult Login()
        {    
            var model = new LoginModel();
            if (Session[CommonConstantUser.USER_SESSION] == null)
            {
                return View(model);
            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            UserDao userDao = new UserDao();
            if(ModelState.IsValid)
            {
                if (userDao.KTTaiKhoan(model.Username) && userDao.NotAdmin(model.Username))
                {
                    if (userDao.KTMatKhau(Encryptor.MD5Hash(model.Password), model.Username))
                    {
                        var user = userDao.getInfo(model.Username);
                        var userSession = new UserInfo();
                        userSession.Id = user.id;
                        userSession.TaiKhoan = user.Username;
                        userSession.Image = user.Image;
                        userSession.HoTen = user.Name;
                        Session.Add(CommonConstantUser.USER_SESSION, userSession);
                        return Redirect("/");
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

        public ActionResult InstallmentTerm()
        {
            return View();
        }

        public ActionResult TransportTerm()
        {
            return View();
        }

        public ActionResult WarrantyTerm()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }


        public ActionResult DetailProduct(int id)
        {
            var model = (from a in data.ProductImages
                         join b in data.Products on a.Product_id equals b.id
                         join c in data.Brands on b.Brand_id equals c.Id
                         where b.id == id && a.IsDeleted == false
                         select new DetailProductViewModel
                         {
                             id = b.id,
                             ProductName=b.ProductName,
                             ProductType_id = b.ProductType_id,
                             Price =b.Price,
                             MonthWarranty=b.MonthWarranty,
                             Url=a.Url,
                             BrandName=c.BrandName,
                             Info = b.Info,
                             Amount = b.Amount
                         }).ToList();
            var cate = (from a in data.ProducTypes
                        join b in data.Products on a.Id equals b.ProductType_id
                        where b.id == id
                        select new { a.TypeName,a.Info,a.Metatitle}).FirstOrDefault();

            ViewBag.Category = cate.TypeName;
            ViewBag.CategoryHref = cate.Metatitle;
            return View(model);
        }
        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            var model = new SignUpViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userDao = new UserDao();
                if(userDao.KTEmail(model.Email)==false && userDao.KTTaiKhoan(model.Username)==false)
                {
                    var user = new User();
                    user.Email = model.Email;
                    user.Username = model.Username;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Image = "";
                    user.IsDeleted = true;
                    user.CreateDate = DateTime.Now;
                    userDao.InsertUser(user);

                    var userInf = userDao.getInfo(model.Username);
                    var userSession = new UserInfo();
                    userSession.Id = userInf.id;
                    userSession.TaiKhoan = userInf.Username;
                    userSession.Image = userInf.Image;
                    userSession.HoTen = userInf.Name;
                    Session.Add(CommonConstantUser.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "Email hoặc tài khoản đã tồn tại");
                }
            }
            return View(model);
        }

        public JsonResult LoadItems(int Tid, int Pid)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.Products
                     where a.IsDeleted == false && a.ProductType_id == Tid && a.id != Pid
                     select new ProductCategoryViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName,
                         Info = a.Info,
                         Price = a.Price,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OpenItemDoc(int id)
        {
            var model = new List<ProductCategoryViewModel>();
            model = (from a in data.Products
                     where a.IsDeleted == false && a.id == id
                     select new ProductCategoryViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName,
                         Info = a.Info,
                         Price = a.Price,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductCategoryViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Info = item.Info,
                    Price = item.Price,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}