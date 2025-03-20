using System.Web.Optimization;

namespace WebA
{
    public class BundleConfig
    {
        // Метод для регистрации всех бандлов
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Бандл для стилей Bootstrap
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css")
                .Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform()));

            // Бандл для скриптов Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js")
                .Include("~/Scripts/bootstrap.min.js"));

            // Бандл для jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            // Можно добавить дополнительные бандлы для других стилей или скриптов
            // Например, bundles.Add(new StyleBundle("~/bundles/styles").Include("~/Content/styles.css"));
        }
    }
}
