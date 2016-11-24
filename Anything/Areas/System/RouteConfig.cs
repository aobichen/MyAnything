using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.System
{
    internal static class RouteConfig
    {
        internal static void RegisterRoutes(AreaRegistrationContext context)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           

            context.MapRoute(
               "System",
               "System/{controller}/{action}/{id}",
               new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               new string[] { "Anything.Areas.System.Controllers" }
           );

        }
    }
}