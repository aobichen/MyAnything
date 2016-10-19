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
        protected AnythingEntities _db;
        public const double _AllPay = 3.5*0.01;
        public  double _Bussiness = 5 * 100;
        public  double _Platform = 5 * 100;
        public double _ParentLine = 0;
        public BaseController()
            : base()
        {
            _db = new AnythingEntities();

            //var bouns = _db.BonusSystem.FirstOrDefault();
            //if (bouns == null)
            //{
            //    _Bussiness = 5 * 0.01;
            //    _Platform = 2.5 * 0.01;
            //    _ParentLine = 2.5 * 0.01;
            //}
            //else
            //{
            //    //交易時消費者獲得的紅利
            //    _Bussiness = bouns.UserBonus;
            //    //平台紅利
            //    _Platform = bouns.ParentBonus / 2;
            //    //上線的紅利 / 6
            //    _ParentLine = bouns.ParentBonus / 2;
            //}

        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string name = requestContext.HttpContext.User.Identity.Name;
                UserId = requestContext.HttpContext.User.Identity.GetUserId<int>();
            }
            
            if (requestContext.RouteData.Values["username"] != null)
            {
                var username = requestContext.RouteData.Values["username"].ToString();
                Session["WebName"] = username;
                var user = Account_db.Users.Where(o => o.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    RecommendCode = user.UserCode;
                    Session["RecommendCode"] = RecommendCode;
                }
            }
        }

        private int UserId { get; set; }
        protected string RecommendCode { get; set; }

        
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