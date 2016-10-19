using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Controllers
{
    public class AnythingController : BaseController
    {
        //
        // GET: /Anything/
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml", new { username = Session["WebName"].ToString() });
        }
    }
}