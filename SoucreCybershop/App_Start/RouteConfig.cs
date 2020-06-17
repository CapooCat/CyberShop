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
              name: "Search",
              url: "TimKiem/{Value}",
              defaults: new { controller = "Product", action = "Search", Value = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "SearchJSON",
              url: "TimKiem/{Value}/JSON",
              defaults: new { controller = "Product", action = "SearchJSON", Value = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "LessThanJSON",
              url: "san-pham/{Type}-duoi-{Number}-trieu/JSON",
              defaults: new { controller = "Product", action = "SortPriceLessThan", Type = UrlParameter.Optional, Number = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "MoreThanJSON",
              url: "san-pham/{Type}-tren-{Number}-trieu/JSON",
              defaults: new { controller = "Product", action = "SortPriceMoreThan", Type = UrlParameter.Optional, Number = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "FromToJSON",
              url: "san-pham/{type}-tu-{NumberFrom}-den-{NumberTo}-trieu/JSON",
              defaults: new { controller = "Product", action = "SortPriceFromTo", Type = UrlParameter.Optional, NumberFrom = UrlParameter.Optional, NumberTo = UrlParameter.Optional}
            );



            routes.MapRoute(
              name: "LessThan",
              url: "san-pham/{metatitle}-duoi-{Number}-trieu/JSON",
              defaults: new { controller = "Product", action = "CategoryDetailSuggest", metatitle = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "MoreThan",
              url: "san-pham/{metatitle}-tren-{Number}-trieu/JSON",
              defaults: new { controller = "Product", action = "CategoryDetailSuggest", metatitle = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "FromTo",
              url: "san-pham/{metatitle}-tu-{NumberFrom}-den-{NumberTo}-trieu/JSON",
              defaults: new { controller = "Product", action = "CategoryDetailSuggest", metatitle = UrlParameter.Optional }
            );



            routes.MapRoute(
              name: "DanhMuc",
              url: "danh-muc/{metatitle}",
              defaults: new { controller = "Product", action = "Category", metatitle = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "ChiTietDanhMuc",
             url: "chi-tiet-danh-muc/{metatitle}",
             defaults: new { controller = "Product", action = "Category", metatitle = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "SanPham",
             url: "san-pham/{metatitle}",
             defaults: new { controller = "Product", action = "Category", metatitle = UrlParameter.Optional }
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
