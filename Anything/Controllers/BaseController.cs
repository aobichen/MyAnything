using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace Anything.Controllers
{
    public class BaseController : Controller
    {
        protected MyAnythingEntities _db;


        public BaseController()
            : base()
        {
            _db = new MyAnythingEntities();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string name = requestContext.HttpContext.User.Identity.Name;
                UserId = requestContext.HttpContext.User.Identity.GetUserId<int>();
            }

           
            //Session["RecommendCode"] = System.Configuration.ConfigurationManager.AppSettings["OfficalRecommendCode"].ToString();
            
            if (requestContext.RouteData.Values["username"] != null)
            {
                var username = requestContext.RouteData.Values["username"].ToString();
                Session["WebName"] = username;
                var user = Account_db.Users.Where(o => o.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    Session["RecommendCode"] = user.UserCode;
                }
                else
                {
                    Session["RecommendCode"] = System.Configuration.ConfigurationManager.AppSettings["OfficalRecommendCode"].ToString();
                }
            }
        }

        private int UserId { get; set; }
        protected string RecommendCode { get; set; }

        protected string IPaddress { 
            get {
                return Request.UserHostAddress;
        } }
        
        //private ApplicationUserManager _userManager;
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    //private set
        //    //{
        //    //    _userManager = value;
        //    //}
        //}
        
        protected ApplicationDbContext Account_db
        {
            
            get { return new ApplicationDbContext(); }
        }


        //public void a()
        //{
        //    var a = UserManager
        //}

        //protected virtual new CustomPrincipal User
        //{
           
        //    get { return HttpContext.User as CustomPrincipal; }
        //}

        
        protected ApplicationUser CurrentUser
        {
            get
            {

                return Account_db.Users.Where(o => o.Id == UserId).FirstOrDefault();
            }
        }
	}
}