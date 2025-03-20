using System.Web.Mvc;
using System.Web.Routing;

namespace WebA
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Отключаем ненужные маршруты
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Основной маршрут по умолчанию для Home/Index
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
