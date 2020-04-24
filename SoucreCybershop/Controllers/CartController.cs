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
    public class CartController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public EmptyResult AddItem(int id)
        {
            var cart = Session[Common.CommonConstantUser.CART_SESSION];
            var product = data.Products.Where(x => x.id == id).FirstOrDefault();
            if(cart!=null)
            {
                var list = (List<CartViewModel>)cart;
                if(list.Exists(x=>x.id==id))
                {
                    foreach(var item in list)
                    {
                        if(item.id==id)
                        {
                            item.Quanlity += 1;
                            item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                        }
                    }
                    Session[Common.CommonConstantUser.CART_SESSION] = list;
                }
                else
                {
                    var item = new CartViewModel();
                    item.id = id;
                    item.ProductName = data.Products.Where(x => x.id == id).Select(x => x.ProductName).FirstOrDefault();
                    item.Quanlity = 1;
                    item.Image = product.Image;
                    item.Price = product.Price;
                    list.Add(item);
                    Session[Common.CommonConstantUser.CART_SESSION] = list;
                }
                
            }
            else
            {
                var list = new List<CartViewModel>();
                var item = new CartViewModel();
                item.id = id;
                item.ProductName = data.Products.Where(x => x.id == id).Select(x => x.ProductName).FirstOrDefault();
                item.Quanlity = 1;
                item.Image = product.Image;
                item.Price = product.Price;
                list.Add(item);
                Session[Common.CommonConstantUser.CART_SESSION] = list;
            }
            return new EmptyResult();
        }
        public JsonResult ReturnCartItem()
        {
            if (Session[Common.CommonConstantUser.CART_SESSION] != null)
            {
                List<CartViewModel> cartList = (List<CartViewModel>)Session[Common.CommonConstantUser.CART_SESSION];
                List<object> ReturnData = new List<object>();
                foreach (CartViewModel item in cartList)
                {
                    ReturnData.Add(new CartViewModel
                    {
                        id = item.id,
                        ProductName = item.ProductName,
                        Quanlity = item.Quanlity,
                        Image =item.Image,
                        Price = item.Price,
                });
                }
                return Json(ReturnData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }
        public EmptyResult RemoveItem(int id)
        {
            var cart = Session[Common.CommonConstantUser.CART_SESSION];
            List<CartViewModel> cartList = (List<CartViewModel>)cart;
            foreach(CartViewModel item in cartList.ToList())
            {
                if(item.id==id)
                {
                    cartList.Remove(item);
                }
            }
            Session[Common.CommonConstantUser.CART_SESSION] = cartList;
            return new EmptyResult();
        }
        public EmptyResult MiniusItem(int id)
        {
            var cart = Session[Common.CommonConstantUser.CART_SESSION];
            var product = data.Products.Where(x => x.id == id).FirstOrDefault();
            List<CartViewModel> cartList = (List<CartViewModel>)cart;
            foreach (CartViewModel item in cartList)
            {
                if (item.id == id)
                {
                    item.Quanlity -= 1;
                    item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                }
            }
            Session[Common.CommonConstantUser.CART_SESSION] = cartList;
            return new EmptyResult();
        }
    }
}