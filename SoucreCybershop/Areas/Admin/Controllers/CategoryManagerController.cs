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
            model = data.Categories.Where(x=>x.category_lv2_id==null && x.category_lv3_id==null).Select(x => new CategoryManagerViewModel {
                Id = x.Id,
                category_lv2_id = x.category_lv2_id,
                category_lv3_id = x.category_lv3_id,
                Name = x.Name,
                IsDeleted = x.IsDeleted,
                CreateBy=x.CreateBy,
                CreateDate=x.CreateDate,
                Metatitle=x.Metatitle,
                category_lv1_master_id=x.category_lv1_master_id,
                category_lv2_master_id=x.category_lv2_master_id
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
            model = data.Categories.Where(x => x.category_lv2_id == null && x.category_lv3_id == null && x.Id==id).Select(x => new CategoryManagerViewModel
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
            cateLv1.Metatitle = model.Metatitle;
            cateLv1.IsDeleted = false;
            cateLv1.CreateDate = DateTime.Now;
            cateLv1.CreateBy = "Admin";
            if (cateDao.InsertCategoryLv1(cateLv1))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet);}
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public JsonResult AddCategoryLv2(CategoryManagerViewModel model)
        {
            var cateDao = new CategoriesDao();
            var cateLv2 = new Category();
            var catIdLv2 = data.Categories.Where(x => x.category_lv3_id == null && x.category_lv2_master_id == null).OrderByDescending(x => x.category_lv2_id).First().category_lv2_id;
            cateLv2.Name = model.Name;
            cateLv2.Metatitle = model.Metatitle;
            cateLv2.category_lv1_master_id = model.category_lv1_master_id;
            cateLv2.category_lv2_id =catIdLv2+1;
            cateLv2.IsDeleted = false;
            cateLv2.CreateDate = DateTime.Now;
            cateLv2.CreateBy = "Admin"; 
            if (cateDao.InsertCategoryLv1(cateLv2))
            { return Json(new { success = true }, JsonRequestBehavior.AllowGet); }
            else { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
        }
    }
}