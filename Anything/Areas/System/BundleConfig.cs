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
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Content/jquery-localize/dist/jquery.localize.js",
            //            "~/Scripts/jquery.cookie.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/System/Scripts").Include(
                      "~/Scripts/jquery-1.10.2.min.js",
                      "~/Content/bootstrap.min.js",
                      "~/Scripts/customer.min.js"
                     
                      ));


    
            bundles.Add(new StyleBundle("~/System/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/font-awesome-4.6.3/css/font-awesome.min.css",
                    "~/Content/customer.min.css"
              
                    ));
        }
    }
}