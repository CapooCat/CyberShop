using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberShop.Models;
using Data;
namespace CyberShop.Controllers
{
    public class BuildController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: BuildPC
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnPCPart(int id)
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            model = (from a in data.Products
                     join b in data.ProducTypes on a.ProductType_id equals b.Id
                     where a.ProductType_id == id && a.IsDeleted == false
                     select new ProductViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName,
                         TypeName = b.TypeName,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         Price = a.Price,
                         Amount = a.Amount,
                         Image = a.Image,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         IsDeleted = a.IsDeleted,
                         MonthWarranty = a.MonthWarranty,
                         SalePercent = a.SalePercent,
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                if (item.Amount > 0)
                {
                    item.CheckStatus = "còn hàng";
                } else { item.CheckStatus = "hết hàng";  }
                ReturnData.Add(new ProductViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    TypeName = item.TypeName,
                    Price = item.Price,
                    Amount = item.Amount,
                    Image = item.Image,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    IsDeleted = item.IsDeleted,
                    MonthWarranty = item.MonthWarranty,
                    SalePercent = item.SalePercent,
                    CheckStatus = item.CheckStatus
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnPCItem(int id)
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            model = (from a in data.Products
                     join b in data.ProducTypes on a.ProductType_id equals b.Id
                     where a.id == id && a.IsDeleted == false
                     select new ProductViewModel
                     {
                         id = a.id,
                         ProductName = a.ProductName,
                         TypeName = b.TypeName,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         Price = a.Price,
                         Amount = a.Amount,
                         Image = a.Image,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         IsDeleted = a.IsDeleted,
                         MonthWarranty = a.MonthWarranty,
                         SalePercent = a.SalePercent
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductViewModel
                {
                    id = item.id,
                    ProductName = item.ProductName,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    TypeName = item.TypeName,
                    Price = item.Price,
                    Amount = item.Amount,
                    Image = item.Image,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    IsDeleted = item.IsDeleted,
                    MonthWarranty = item.MonthWarranty,
                    SalePercent = item.SalePercent
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
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
                        Image = item.Image,
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
        
        public JsonResult AddAllToCart(List<int> lstId, int qanMAIN, int qanCPU, int qanRAM, int qanSSD, int qanHDD, int qanPSU, int qanVGA, int qanCASE, int qanMONITOR, int qanCOOLER)
        {
            foreach (var id in lstId)
            {
                var cart = Session[Common.CommonConstantUser.CART_SESSION];
                var product = data.Products.Where(x => x.id == id).FirstOrDefault();
                if (cart != null)
                {
                    var list = (List<CartViewModel>)cart;
                    if (list.Exists(x => x.id == id))
                    {
                        foreach (var item in list)
                        {
                            if (item.id == id && product.ProductType_id == 2) //MAIN
                            {
                                item.Quanlity += qanMAIN;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 3) //CPU
                            {
                                item.Quanlity += qanCPU;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 5) //RAM
                            {
                                item.Quanlity += qanRAM;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 9) //SSD
                            {
                                item.Quanlity += qanSSD;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 8) //HDD
                            {
                                item.Quanlity += qanHDD;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 4) //PSU
                            {
                                item.Quanlity += qanPSU;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 1) //VGA
                            {
                                item.Quanlity += qanVGA;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 6) //CASE
                            {
                                item.Quanlity += qanCASE;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 12) //MONITOR
                            {
                                item.Quanlity += qanMONITOR;
                                item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                            }
                            if (item.id == id && product.ProductType_id == 7) //COOLER
                            {
                                item.Quanlity += qanCOOLER;
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
                        if (product.ProductType_id == 2) //MAIN
                        {
                            item.Quanlity = qanMAIN;
                        }
                        if (product.ProductType_id == 3) //CPU
                        {
                            item.Quanlity = qanCPU;
                        }
                        if (product.ProductType_id == 5) //RAM
                        {
                            item.Quanlity = qanRAM;
                        }
                        if (product.ProductType_id == 9) //SSD
                        {
                            item.Quanlity = qanSSD;
                        }
                        if (product.ProductType_id == 8) //HDD
                        {
                            item.Quanlity = qanHDD;
                        }
                        if (product.ProductType_id == 4) //PSU
                        {
                            item.Quanlity = qanPSU;
                        }
                        if (product.ProductType_id == 1) //VGA
                        {
                            item.Quanlity = qanVGA;
                        }
                        if (product.ProductType_id == 6) //CASE
                        {
                            item.Quanlity = qanCASE;
                        }
                        if (product.ProductType_id == 12) //MONITOR
                        {
                            item.Quanlity = qanMONITOR;
                        }
                        if (product.ProductType_id == 7) //COOLER
                        {
                            item.Quanlity = qanCOOLER;
                        }
                        item.Image = product.Image;
                        item.Price = Convert.ToDouble(product.Price * item.Quanlity);
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
                    if (product.ProductType_id == 2) //MAIN
                    {
                        item.Quanlity = qanMAIN;
                    }
                    if (product.ProductType_id == 3) //CPU
                    {
                        item.Quanlity = qanCPU;
                    }
                    if (product.ProductType_id == 5) //RAM
                    {
                        item.Quanlity = qanRAM;
                    }
                    if (product.ProductType_id == 9) //SSD
                    {
                        item.Quanlity = qanSSD;
                    }
                    if (product.ProductType_id == 8) //HDD
                    {
                        item.Quanlity = qanHDD;
                    }
                    if (product.ProductType_id == 4) //PSU
                    {
                        item.Quanlity = qanPSU;
                    }
                    if (product.ProductType_id == 1) //VGA
                    {
                        item.Quanlity = qanVGA;
                    }
                    if (product.ProductType_id == 6) //CASE
                    {
                        item.Quanlity = qanCASE;
                    }
                    if (product.ProductType_id == 12) //MONITOR
                    {
                        item.Quanlity = qanMONITOR;
                    }
                    if (product.ProductType_id == 7) //COOLER
                    {
                        item.Quanlity = qanCOOLER;
                    }
                    item.Image = product.Image;
                    item.Price = Convert.ToDouble(product.Price * item.Quanlity);
                    list.Add(item);
                    Session[Common.CommonConstantUser.CART_SESSION] = list;
                }
            }
            return ReturnCartItem();
        }
    }
}