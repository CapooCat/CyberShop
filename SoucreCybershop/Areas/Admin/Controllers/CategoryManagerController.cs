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
    }
}