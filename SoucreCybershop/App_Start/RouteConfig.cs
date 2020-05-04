using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CyberShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "DanhMuc",
              url: "danh-muc/{metatitle}",
              defaults: new { controller = "Product", action = "Category", metatitle = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "ChiTietDanhMuc",
             url: "chi-tiet-danh-muc/{metatitle}",
             defaults: new { controller = "Product", action = "CategoryDetail", metatitle = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "SanPham",
             url: "san-pham/{metatitle}",
             defaults: new { controller = "Product", action = "CategoryDetailSuggest", metatitle = UrlParameter.Optional }
           );



            routes.MapRoute(
              name: "DanhMucJSON",
              url: "danh-muc/{metatitle}/JSON",
              defaults: new { controller = "Product", action = "CategoryJSON", metatitle = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "ChiTietDanhMucJSON",
             url: "chi-tiet-danh-muc/{metatitle}/JSON",
             defaults: new { controller = "Product", action = "CategoryDetailJSON", metatitle = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "SanPhamJSON",
             url: "san-pham/{metatitle}/JSON",
             defaults: new { controller = "Product", action = "CategoryDetailSuggestJSON", metatitle = UrlParameter.Optional }
           );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
