using Anything.Controllers;
using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anything.Areas.System.Controllers
{
    public class HomeController : BaseController
    {
        // GET: System/Home
        public ActionResult Index()
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            ViewBag.UserName = CurrentUser.UserName;
            var Logs = _db.SystemLog.Where(o => o.Creator == CurrentUser.Email && o.LogCode == "Time" && o.LogType == "SignIn").OrderBy(o => o.Created).ToList();
            var Login = new SystemLog();
            var IsFirst = "0";
            if (Logs != null && Logs.Count >= 2)
            {
                Login = Logs[1];
                IsFirst = "1";
            }
            else if (Logs != null && Logs.Count == 1)
            {
                Login = Logs[0];
            }
            ViewBag.IsFirst = IsFirst;
            ViewBag.IP = Login.IP;
            ViewBag.Time = Login.Created.Value.ToString("yyyy-MM-dd HH:mm");
            ViewBag.ReturnUrl = "/System";
            return View();
        }
    }
}