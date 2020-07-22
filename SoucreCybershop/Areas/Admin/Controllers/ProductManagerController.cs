using System;
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
                     where a.IsDeleted == false
                     select new ProductTypeManagerViewModel
                     {
                         id = a.Id,
                         TypeName = a.TypeName,
                         MetaTitle = a.Metatitle
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new ProductTypeManagerViewModel
                {
                    id = item.id,
                    TypeName = item.TypeName,
                    MetaTitle = item.MetaTitle
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
                     where c.IsDeleted == false
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
            Type.IsDeleted = false;
            Type.CreateTime = DateTime.Now;
            Type.CreateBy = "Admin";
            if (TypeDao.InsertType(Type))
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
                                      .Where(x => ((model.ProductType_id == null) || (x.ProductType_id == model.ProductType_id)))
                                      .ToList();
            if (model.PriceTo != null && model.PriceFrom == null)
            {
                lstProduct = lstProduct.Where(x => x.Price == model.PriceTo).ToList();
            }
            if (model.PriceTo == null && model.PriceFrom != null)
            {
                lstProduct = lstProduct.Where(x => x.Price == model.PriceFrom).ToList();
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
        public JsonResult ReturnImage(int id)
        {
            var model = new List<ProductImageManagerViewModel>();
            model = (from a in data.Products
                     join b in data.ProductImages on a.id equals b.Product_id
                     where a.IsDeleted == false && b.Product_id==id
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
                    var pdDao = new ProductImageDao().InsertImage(entity);
                    file.SaveAs(path);
                }
            }
            return ReturnImage(lstImage.Product_Id);
        }
        public JsonResult ReturnHistory()
        {
            var model = new List<HistoryViewModel>();
            model = (from b in data.ProducTypes
                     join a in data.Products on b.Id equals a.ProductType_id
                     join c in data.Histories on a.id equals c.Product_id
                     where c.IsDeleted == false
                     select new HistoryViewModel
                     {
                         Id = c.Id,
                         User_id = c.User_id,
                         Description = c.Description,
                         ProductTypeName = b.TypeName,
                         ProductName = a.ProductName,
                         Product_tag = c.Product_tag,
                         Invoice_tag = c.Invoice_tag,
                         Warehouse_tag = c.Warehouse_tag,
                         User_tag = c.User_tag,
                         IsDeleted = c.IsDeleted,
                         Product_id = c.Product_id,
                         CreateDate = a.CreateDate,
                         Amount = a.Amount,
                         TotalPrice = c.TotalPrice
                     }).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new HistoryViewModel
                {
                    Id = item.Id,
                    User_id = item.User_id,
                    Description = item.Description,
                    ProductTypeName = item.ProductTypeName,
                    ProductName = item.ProductName,
                    Product_tag = item.Product_tag,
                    Invoice_tag = item.Invoice_tag,
                    Warehouse_tag = item.Warehouse_tag,
                    User_tag = item.User_tag,
                    IsDeleted = item.IsDeleted,
                    Product_id = item.Product_id,
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