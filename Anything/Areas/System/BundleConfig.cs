using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Anything.Areas.System
{
    internal static class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/jquery-localize/dist/jquery.localize.js",
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/System/bundles/bootstrap").Include(
                      "~/Content/jquery.min.js",
                      "~/Content/bootstrap.min.js"
                     
                      ));


    //        <link href="../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    //<!-- Font Awesome -->
    //<link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    //<!-- NProgress -->
    //<link href="../vendors/nprogress/nprogress.css" rel="stylesheet">
    //<!-- iCheck -->
    //<link href="../vendors/iCheck/skins/flat/green.css" rel="stylesheet">
    //<!-- bootstrap-progressbar -->
    //<link href="../vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css" rel="stylesheet">
    //<!-- JQVMap -->
    //<link href="../vendors/jqvmap/dist/jqvmap.min.css" rel="stylesheet"/>
    //<!-- bootstrap-daterangepicker -->
    //<link href="../vendors/bootstrap-daterangepicker/daterangepicker.css" rel="stylesheet">

            bundles.Add(new StyleBundle("~/Admin/Content/css").Include(
                    "~/Content/vendors/bootstrap.css",
                    "~/Content/font-awesome-4.6.3/css/font-awesome.min.css"
              
                    ));
        }
    }
}