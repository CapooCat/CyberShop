using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberShop.Areas.Admin.Models;
using Data;
namespace CyberShop.Areas.Admin.Controllers
{
    public class CategoryManagerController : Controller
    {
        // GET: Admin/CategoryManager
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnCategory()
        {
            var model = new List<CategoryManagerViewModel>();
            model = data.Categories.Where(x => x.category_lv2_id == null && x.category_lv3_id == null).Select(x => new CategoryManagerViewModel
            {
                Id = x.Id,
                category_lv2_id = x.category_lv2_id,
                category_lv3_id = x.category_lv3_id,
                Name = x.Name,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                Metatitle = x.Metatitle,
                category_lv1_master_id = x.category_lv1_master_id,
                category_lv2_master_id = x.category_lv2_master_id
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new CategoryManagerViewModel
                {
                    Id = item.Id,
                    category_lv2_id = item.category_lv2_id,
                    category_lv3_id = item.category_lv3_id,
                    Name = item.Name,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Metatitle = item.Metatitle,
                    category_lv1_master_id = item.category_lv1_master_id,
                    category_lv2_master_id = item.category_lv2_master_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnCategoryLv2(int id) //category_id_lv1
        {
            var model = new List<CategoryManagerViewModel>();
            model = data.Categories.Where(x => x.category_lv1_master_id == id && x.category_lv3_id == null).Select(x => new CategoryManagerViewModel
            {
                Id = x.Id,
                category_lv2_id = x.category_lv2_id,
                category_lv3_id = x.category_lv3_id,
                Name = x.Name,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                Metatitle = x.Metatitle,
                category_lv1_master_id = x.category_lv1_master_id,
                category_lv2_master_id = x.category_lv2_master_id
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new CategoryManagerViewModel
                {
                    Id = item.Id,
                    category_lv2_id = item.category_lv2_id,
                    category_lv3_id = item.category_lv3_id,
                    Name = item.Name,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Metatitle = item.Metatitle,
                    category_lv1_master_id = item.category_lv1_master_id,
                    category_lv2_master_id = item.category_lv2_master_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnCategoryLv3(int id) //category_id_lv2
        {
            var model = new List<CategoryManagerViewModel>();
            model = data.Categories.Where(x => x.category_lv2_master_id == id).Select(x => new CategoryManagerViewModel
            {
                Id = x.Id,
                category_lv2_id = x.category_lv2_id,
                category_lv3_id = x.category_lv3_id,
                Name = x.Name,
                IsDeleted = x.IsDeleted,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                Metatitle = x.Metatitle,
                category_lv1_master_id = x.category_lv1_master_id,
                category_lv2_master_id = x.category_lv2_master_id
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new CategoryManagerViewModel
                {
                    Id = item.Id,
                    category_lv2_id = item.category_lv2_id,
                    category_lv3_id = item.category_lv3_id,
                    Name = item.Name,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    Metatitle = item.Metatitle,
                    category_lv1_master_id = item.category_lv1_master_id,
                    category_lv2_master_id = item.category_lv2_master_id
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnNameCategory(int id) //category_id_lv1
        {
            var model = new List<CategoryManagerViewModel>();
            model = data.Categories.Where(x => x.category_lv2_id == null && x.category_lv3_id == null && x.Id == id).Select(x => new CategoryManagerViewModel
            {
                Name = x.Name,
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new CategoryManagerViewModel
                {
                    Name = item.Name,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnNameCategoryLv2(int id) //category_id_lv2
        {
            var model = new List<CategoryManagerViewModel>();
            model = data.Categories.Where(x => x.category_lv2_id == id && x.category_lv3_id == null).Select(x => new CategoryManagerViewModel
            {
                Name = x.Name,
            }).ToList();

            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new CategoryManagerViewModel
                {
                    Name = item.Name,
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddCategory(CategoryManagerViewModel model)
        {
            var cateDao = new CategoriesDao();
            var cateLv1 = new Category();
            cateLv1.Name = model.Name;
            cateLv1.Metatitle = model.ProductTypeKW;
            if (!String.IsNullOrEmpty(model.BrandKW))
            {
                cateLv1.Metatitle = cateLv1.Metatitle + "-" + model.BrandKW;
            }
            if (!String.IsNullOrEmpty(model.ProductKW))
            {
                cateLv1.Metatitle = cateLv1.Metatitle + "-" + model.ProductKW;
            }
            if (model.LowPrice != null && model.HighPrice != null)
            {
                cateLv1.Metatitle = cateLv1.Metatitle + "-" + "tu-" + model.LowPrice + "-den-" + model.HighPrice + "-trieu";
            }
            cateLv1.IsDeleted = false;
            cateLv1.CreateDate = DateTime.Now;
            cateLv1.CreateBy = "Admin";
            if (cateDao.InsertCategoryLv1(cateLv1))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public JsonResult AddCategoryLv2(CategoryManagerViewModel model)
        {
            var cateDao = new CategoriesDao();
            var cateLv2 = new Category();
            var catIdLv2 = data.Categories.Where(x => x.category_lv3_id == null && x.category_lv2_master_id == null).OrderByDescending(x => x.category_lv2_id).First().category_lv2_id;
            cateLv2.Name = model.Name;
            cateLv2.Metatitle = model.ProductTypeKW;
            if (!String.IsNullOrEmpty(model.BrandKW))
            {
                cateLv2.Metatitle = cateLv2.Metatitle + "-" + model.BrandKW;
            }
            if (!String.IsNullOrEmpty(model.ProductKW))
            {
                cateLv2.Metatitle = cateLv2.Metatitle + "-" + model.ProductKW;
            }
            if (model.LowPrice != null && model.HighPrice != null)
            {
                cateLv2.Metatitle = cateLv2.Metatitle + "-" + "tu-" + model.LowPrice + "-den-" + model.HighPrice + "-trieu";
            }
            cateLv2.category_lv1_master_id = model.category_lv1_master_id;
            cateLv2.category_lv2_id = catIdLv2 + 1;
            cateLv2.IsDeleted = false;
            cateLv2.CreateDate = DateTime.Now;
            cateLv2.CreateBy = "Admin";
            if (cateDao.InsertCategoryLv1(cateLv2))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public JsonResult AddCategoryLv3(CategoryManagerViewModel model)
        {
            var cateDao = new CategoriesDao();
            var cateLv3 = new Category();
            var catIdLv3 = data.Categories.Where(x => x.category_lv2_id == null).OrderByDescending(x => x.category_lv3_id).First().category_lv3_id;
            cateLv3.Name = model.Name;
            cateLv3.Metatitle = model.ProductTypeKW;
            if (!String.IsNullOrEmpty(model.BrandKW))
            {
                cateLv3.Metatitle = cateLv3.Metatitle + "-" + model.BrandKW;
            }
            if (!String.IsNullOrEmpty(model.ProductKW))
            {
                cateLv3.Metatitle = cateLv3.Metatitle + "-" + model.ProductKW;
            }
            if (model.LowPrice != null && model.HighPrice != null)
            {
                cateLv3.Metatitle = cateLv3.Metatitle + "-" + "tu-" + model.LowPrice + "-den-" + model.HighPrice + "-trieu";
            }
            cateLv3.category_lv1_master_id = model.category_lv1_master_id;
            cateLv3.category_lv2_master_id = model.category_lv2_master_id;
            cateLv3.category_lv3_id = catIdLv3 + 1;
            cateLv3.IsDeleted = false;
            cateLv3.CreateDate = DateTime.Now;
            cateLv3.CreateBy = "Admin";
            if (cateDao.InsertCategoryLv1(cateLv3))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        public JsonResult ReturnBrandList()
        {
            var model = new List<Brand>();
            model = data.Brands.ToList();
            List<object> ReturnData = new List<object>();
            foreach (var item in model)
            {
                ReturnData.Add(new Brand
                {
                    Id = item.Id,
                    BrandName = item.BrandName,
                    Address = item.Address,
                    PhoneNumber = item.PhoneNumber,
                    Info = item.Info,
                    IsDeleted = item.IsDeleted,
                    CreateBy = item.CreateBy,
                    CreateTime = item.CreateTime
                });
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
    }
}