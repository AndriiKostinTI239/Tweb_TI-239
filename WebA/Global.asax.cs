using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebA
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Регистрация всех маршрутов
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Регистрация бандлов
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
