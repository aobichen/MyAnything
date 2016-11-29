using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using System.Web;
namespace Anything
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "User",
              url: "{username}'sAnything",
              defaults: new { controller = "Anything", action = "Index", username = "" }
          );


            routes.MapRoute(
                name: "Room",
                url: "Room/{id}",
                defaults: new { controller = "Room", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Detail",
            //    url: "Detail/{id}",
            //    defaults: new { controller = "Home", action = "Detail", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
               name: "Booking",
               url: "Booking/{id}",
               defaults: new { controller = "Home", action = "Booking", id = UrlParameter.Optional },
              namespaces: new string[] { "Anything.Controllers" }
           );

         //   routes.MapRoute(
         //    name: "Account/Join",
         //    url: "Account/{action}",
         //    defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
         //);

         //   routes.MapRoute(
         //       name: "Register",
         //       url: "Account/{action}/{id}",
         //       defaults: new { controller = "Account", action = "HotelRegister", id = UrlParameter.Optional }
         //   );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , namespaces: new string[] { "Anything.Controllers" }
            );


            

            

            
        }
    }

    public class SubdomainRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Request == null || httpContext.Request.Url == null)
            {
                return null;
            }

            var host = httpContext.Request.Url.Host;
            var index = host.IndexOf(".");
            string[] segments = httpContext.Request.Url.PathAndQuery.TrimStart('/').Split('/');

            if (index < 0)
            {
                return null;
            }

            var subdomain = host.Substring(0, index);
            string[] blacklist = { "www", "yourdomain", "mail" };

            if (blacklist.Contains(subdomain))
            {
                return null;
            }

            string controller = (segments.Length > 0) ? segments[0] : "Home";
            string action = (segments.Length > 1) ? segments[1] : "Index";

            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.Values.Add("controller", controller); //Goes to the relevant Controller  class
            routeData.Values.Add("action", action); //Goes to the relevant action method on the specified Controller
            routeData.Values.Add("subdomain", subdomain); //pass subdomain as argument to action method
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //Implement your formating Url formating here
            return null;
        }
    }
}