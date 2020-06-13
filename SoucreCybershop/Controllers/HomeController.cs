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
            List<ProductHomeViewModel> spGiamGia = new List<ProductHomeViewModel>();
            spGiamGia = data.Products.Take(12).Select(x => new ProductHomeViewModel
            {
                id = x.id,
                ProductName = x.ProductName,
                oldPrice = x.Price,
                Image = x.Image,
            }).ToList();
            ViewBag.SpGiamGia = spGiamGia;
            List<ProductHomeViewModel> spMoi = new List<ProductHomeViewModel>();
            spMoi = data.Products.Take(12).Select(x=> new ProductHomeViewModel {
                id=x.id,
                ProductName=x.ProductName,
                oldPrice=x.Price,
                Image=x.Image,
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
        public ActionResult DetailProduct(int id)
        {
            var model = (from a in data.ProductImages
                         join b in data.Products on a.Product_id equals b.id
                         join c in data.Brands on b.Brand_id equals c.Id
                         where b.id == id
                         select new DetailProductViewModel
                         {
                             id = b.id,
                             ProductName=b.ProductName,
                             Price=b.Price,
                             MonthWarranty=b.MonthWarranty,
                             Url=a.Url,
                             BrandName=c.BrandName
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
                    userSession.DateOfBirth = userInf.DayOfBirth.ToString();
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
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [AllowAnonymous]
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret=ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri=RedirectUri.AbsoluteUri,
                response_type="code",
                scope="email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code,
            });
            var accessToken = result.access_token;
            fb.AccessToken = accessToken;
            if(!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email,picture");
                string email = me.email;
                string username = me.email;
                string birthday = me.birthday;
                string image = me.picture.data.url;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var userDao = new UserDao();
                if (userDao.KTEmail(email)!=true)
                {
                    var user = new User();
                    user.Email = email;
                    user.Username = email;
                    user.Name = firstname + " " + middlename + " " + lastname;
                    user.IsDeleted = false;
                    user.CreateDate = DateTime.Now;

                    var resultInsert=userDao.InsertUser(user);
                    if(resultInsert==true)
                    {
                        user = userDao.getInfo(user.Email);
                        var userSession = new UserInfo();
                        userSession.Id = user.id;
                        userSession.TaiKhoan = "";
                        userSession.Image = user.Image;
                        userSession.HoTen = user.Name;
                        userSession.DateOfBirth = null;
                        Session.Add(CommonConstantUser.USER_SESSION, userSession);  
                    }
                }
                else
                {
                    var user = new User();
                    user.Email = email;
                    user = userDao.getInfo(user.Email);
                    var userSession = new UserInfo();
                    userSession.Id = user.id;
                    userSession.TaiKhoan = user.Username;
                    userSession.Image = user.Image;
                    userSession.HoTen = user.Name;
                    userSession.DateOfBirth = null;
                    Session.Add(CommonConstantUser.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }
    }
}