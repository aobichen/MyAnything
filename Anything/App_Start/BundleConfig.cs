using System.Web.Optimization;

namespace Anything
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Content/jquery-localize/dist/jquery.localize.js",
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/lightslider.min.js",
                        "~/Scripts/jquery.uniform.min.js",
                        "~/Scripts/jquery.slimmenu.min.js",
                        "~/Scripts/scripts.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                // "~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/lightslider.min.css",
                      "~/Content/theme.css",
                      "~/Content/font-awesome-4.6.3/css/font-awesome.min.css"));
        }
            
    }
}
