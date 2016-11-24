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
               "System_default",
               "System/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );

        }
    }
}