﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using CyberShop.Areas.Admin.Models;
using System.IO;
namespace CyberShop.Areas.Admin.Controllers
{
    public class ProductManagerController : Controller
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        // GET: Admin/ProductManager
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult ReturnBrand()
        {
            var model = new List<BrandManagerViewModel>();
            model = (from a in data.Brands
                     where a.IsDeleted == false
                     select new BrandManagerViewModel
                     {
                         id = a.Id,
                         BrandName = a.BrandName,
                         MetaTitle = a.Metatitle
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new BrandManagerViewModel
                {
                    id = item.id,
                    BrandName = item.BrandName,
                    MetaTitle = item.MetaTitle
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnProductType()
        {
            var model = new List<ProductTypeManagerViewModel>();
            model = (from a in data.ProducTypes
                     join b in data.Categories on a.Category_id equals b.Id
                     where a.IsDeleted == false
                     select new ProductTypeManagerViewModel
                     {
                         id = a.Id,
                         TypeName = a.TypeName,
                         MetaTitle = a.Metatitle,
                         CategoryName = b.Name
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductTypeManagerViewModel
                {
                    id = item.id,
                    TypeName = item.TypeName,
                    MetaTitle = item.MetaTitle,
                    CategoryName = item.CategoryName
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ReturnProduct()
        {
            var model = new List<ProductManagerViewModel>();
            model = (from b in data.ProducTypes
                     join a in data.Products on b.Id equals a.ProductType_id
                     join c in data.Brands on a.Brand_id equals c.Id
                     where a.IsDeleted == false
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         ProductTypeName=b.TypeName,
                         ProductName = a.ProductName,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         Price = a.Price,
                         SalePercent = a.SalePercent,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         Amount = a.Amount,
                         BrandName=c.BrandName
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    ProductTypeName = item.ProductTypeName,
                    ProductName = item.ProductName,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    Price = item.Price,
                    SalePercent = item.SalePercent,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Amount = item.Amount,
                    BrandName=item.BrandName
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductUpdate(ProductManagerViewModel model)
        {
            Product entity = new Product();
            entity = data.Products.Find(model.id);
            entity.Brand_id = model.Brand_id;
            entity.ProductType_id = model.ProductType_id;
            entity.ProductName = model.ProductName;
            entity.Amount = model.Amount;
            entity.MetaTitle = model.MetaTitle;
            entity.Info = model.Info;
            entity.Price = model.Price;
            entity.MonthWarranty = model.MonthWarranty;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnProductById(int id)
        {
            var model = new List<ProductManagerViewModel>();
            model = (from b in data.ProducTypes
                     join a in data.Products on b.Id equals a.ProductType_id
                     join c in data.Brands on a.Brand_id equals c.Id
                     where c.IsDeleted == false && a.id == id
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         ProductTypeName = b.TypeName,
                         ProductName = a.ProductName,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         Price = a.Price,
                         SalePercent = a.SalePercent,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         Amount = a.Amount,
                         BrandName = c.BrandName
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    ProductTypeName = item.ProductTypeName,
                    ProductName = item.ProductName,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    Price = item.Price,
                    SalePercent = item.SalePercent,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Amount = item.Amount,
                    BrandName = item.BrandName
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddBrand(BrandManagerViewModel model)
        {
            var BrandDao = new BrandDao();
            var Brand = new Brand();

            Brand.BrandName = model.BrandName;
            Brand.Metatitle = model.MetaTitle;
            Brand.IsDeleted = false;
            Brand.CreateTime = DateTime.Now;
            Brand.CreateBy = "Admin";
            if (BrandDao.InsertBrand(Brand))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AddType(ProductTypeManagerViewModel model)
        {
            var TypeDao = new ProductTypeDao();
            var Type = new ProducType();
            
            Type.TypeName = model.TypeName;
            Type.Metatitle = model.MetaTitle;
            Type.Category_id = model.Category_id;
            Type.IsDeleted = false;
            Type.CreateTime = DateTime.Now;
            Type.CreateBy = "Admin";
            if (TypeDao.InsertType(Type))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AddProduct(ProductManagerViewModel model)
        {
            var ProductDao = new ProductDao();
            var Product = new Product();

            Product.ProductName = model.ProductName;
            Product.Brand_id = model.Brand_id;
            Product.ProductType_id = model.ProductType_id;
            Product.Price = model.Price;
            Product.Amount = model.Amount;
            Product.Info = model.Info;
            Product.MonthWarranty = model.MonthWarranty;
            Product.MetaTitle = model.MetaTitle;
            Product.IsDeleted = false;
            Product.CreateDate = DateTime.Now;
            Product.CreateBy = "Admin";
            if (ProductDao.InsertProduct(Product)) {
                if (model.ProductType_id == 20)
                {
                    var Detail_PcSetDao = new Detail_PcSetDao();
                    var PCset = new Detail_PcSets();
                    int Id = data.Products.Max(x => x.id);
                    PCset.product_id = Id;
                    PCset.IsDeleted = false;
                    if (Detail_PcSetDao.InsertPC(PCset))
                    { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
                } else
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                
            }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AddImage(ProductImageManagerViewModel model)
        {
            var ProductImageDao = new ProductImageDao();
            var ProductImage = new ProductImage();

            ProductImage.Product_id = model.Product_id;
            ProductImage.Name = model.Name;
            ProductImage.Url = model.Url;
            ProductImage.IsDeleted = false;
            if (ProductImageDao.InsertImage(ProductImage))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }


        public JsonResult DeleteBrand(int id)
        {
            Brand entity = new Brand();
            entity = data.Brands.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteType(int id)
        {
            ProducType entity = new ProducType();
            entity = data.ProducTypes.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FilterProduct(ProductManagerViewModel model)
        {
            var lstProduct = new List<ProductManagerViewModel>();
            lstProduct = (from b in data.ProducTypes
                     join a in data.Products on b.Id equals a.ProductType_id
                     join c in data.Brands on a.Brand_id equals c.Id
                     select new ProductManagerViewModel
                     {
                         id = a.id,
                         Brand_id = a.Brand_id,
                         ProductType_id = a.ProductType_id,
                         ProductTypeName = b.TypeName,
                         ProductName = a.ProductName,
                         MetaTitle = a.MetaTitle,
                         Info = a.Info,
                         Price = a.Price,
                         SalePercent = a.SalePercent,
                         MonthWarranty = a.MonthWarranty,
                         Image = a.Image,
                         IsDeleted = a.IsDeleted,
                         CreateBy = a.CreateBy,
                         CreateDate = a.CreateDate,
                         Amount = a.Amount,
                         BrandName = c.BrandName
                     }).ToList();
            lstProduct = lstProduct.Where(x => ((model.id == null) || (x.id == model.id)))
                                      .Where(x => ((model.ProductName == null) || (x.ProductName.Contains(model.ProductName))))
                                      .Where(x => ((model.BrandName == null) || (x.BrandName.Contains(model.BrandName))))
                                      .Where(x => ((model.ProductType_id == 0) || (x.ProductType_id == model.ProductType_id)))
                                      .ToList();
            if (model.PriceTo != null && model.PriceFrom == null)
            {
                lstProduct = lstProduct.Where(x => x.Price <= model.PriceTo).ToList();
            }
            if (model.PriceTo == null && model.PriceFrom != null)
            {
                lstProduct = lstProduct.Where(x => x.Price >= model.PriceFrom).ToList();
            }
            if (model.PriceTo != null && model.PriceFrom != null)
            {
                lstProduct = lstProduct.Where(x => x.Price >= model.PriceFrom && x.Price <= model.PriceTo).ToList();
            }
            lstProduct = lstProduct.Where(x => x.IsDeleted == false).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in lstProduct)
            {
                ReturnData.Add(new ProductManagerViewModel
                {
                    id = item.id,
                    Brand_id = item.Brand_id,
                    ProductType_id = item.ProductType_id,
                    ProductTypeName = item.ProductTypeName,
                    ProductName = item.ProductName,
                    MetaTitle = item.MetaTitle,
                    Info = item.Info,
                    Price = item.Price,
                    SalePercent = item.SalePercent,
                    MonthWarranty = item.MonthWarranty,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Amount = item.Amount,
                    BrandName = item.BrandName
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public class ImageFile
        {
            public int Product_Id { get; set; }
            public List<HttpPostedFileBase> files { get; set; }
        }

        public class TempImage
        {
            public List<HttpPostedFileBase> files { get; set; }
        }
        public JsonResult ReturnImage(int id)
        {
            var model = new List<ProductImageManagerViewModel>();
            model = (from a in data.Products
                     join b in data.ProductImages on a.id equals b.Product_id
                     where b.IsDeleted == false && b.Product_id==id
                     select new ProductImageManagerViewModel
                     {
                         id = b.id,
                         Name = b.Name,
                         Url = b.Url
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductImageManagerViewModel
                {
                    id = item.id,
                    Name = item.Name,
                    Url = item.Url
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteImage(int id)
        {
            ProductImage entity = new ProductImage();
            entity = data.ProductImages.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadImage(ImageFile lstImage)
        {
            foreach (var file in lstImage.files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/assets/product_image"), file.FileName);
                    ProductImage entity = new ProductImage();
                    entity.Name = file.FileName;
                    entity.Url = "/assets/product_image/"+file.FileName;
                    entity.Product_id = lstImage.Product_Id;
                    entity.IsDeleted = false;
                    var pdDao = new ProductImageDao().InsertImage(entity);
                    file.SaveAs(path);
                }
            }
            return ReturnImage(lstImage.Product_Id);
        }

        public JsonResult AddTempImage(TempImage lstImage)
        {
            var list = new List<TempImagesViewModel>();
            foreach (var file in lstImage.files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/assets/product_image"), file.FileName);
                    var item = new TempImagesViewModel();
                    item.Name = file.FileName;
                    item.Url = "/assets/product_image/" + file.FileName;
                    list.Add(item);
                    file.SaveAs(path);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MainImage(ProductManagerViewModel model)
        {
            Product entity = new Product();
            entity = data.Products.Find(model.id);
            entity.Image= model.Image;
            data.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnHistory()
        {
            var model = new List<HistoryViewModel>();
            model = (from b in data.ProducTypes
                     join a in data.Products on b.Id equals a.ProductType_id
                     join c in data.InOrder_Detail on a.id equals c.Product_id
                     join d in data.InOrders on c.InOrder_id equals d.Id
                     where c.IsDeleted == false && d.Status == "Đã hoàn thành"
                     select new HistoryViewModel
                     {
                         Id = c.Id,
                         ProductTypeName = b.TypeName,
                         ProductName = a.ProductName,
                         CreateDate = d.CreateDate,
                         Amount = c.Amount,
                         TotalPrice = c.Amount * c.Price
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new HistoryViewModel
                {
                    Id = item.Id,
                    ProductTypeName = item.ProductTypeName,
                    ProductName = item.ProductName,
                    CreateDate = item.CreateDate,
                    Amount = item.Amount,
                    TotalPrice = item.TotalPrice
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProductChecked(List<int> id)
        {
            Product entity = new Product();
            foreach (var item in id)
            {
                entity = data.Products.Find(item);
                entity.IsDeleted = true;
                data.SaveChanges();
            }
            return ReturnProduct();
        }
        public JsonResult DeleteProduct(int id)
        {
            Product entity = new Product();
            entity = data.Products.Find(id);
            entity.IsDeleted = true;
            data.SaveChanges();
            return ReturnProduct();
        }

    }
}