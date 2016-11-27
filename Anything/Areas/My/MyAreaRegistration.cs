using System.Web.Mvc;

namespace Anything.Areas.My
{
    public class MyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "My";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "My",
                "My/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}