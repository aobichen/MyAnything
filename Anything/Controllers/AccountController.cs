﻿using System.Globalization;
using Anything.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Anything.Helpers;
using System.Collections.Generic;
using Facebook;

namespace Anything.Controllers
{

    public class AccountController : BaseController
    {
        private string OfficalRecommendCode { get; set; }
        public AccountController()
        {
           OfficalRecommendCode =  System.Configuration.ConfigurationManager.AppSettings["OfficalRecommendCode"].ToString();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            AuthenticationManager.SignOut();
            ViewBag.ReturnUrl = returnUrl;
           
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            model.Password = model.Password.ToLower();
            FormsAuthentication.SignOut();
            AuthenticationManager.SignOut();
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            //比對驗證碼
            if (System.Configuration.ConfigurationManager.AppSettings["VerificationCode"] != null)
            {
                var VerificationText = System.Configuration.ConfigurationManager.AppSettings["VerificationCode"];
                var VerificationCode = Session[VerificationText].ToString();
                if (string.IsNullOrEmpty(model.VerificationCode) || VerificationCode.ToUpper() != model.VerificationCode.ToUpper())
                {
                    ModelState.AddModelError("", "無效的驗證碼");
                    return View();
                }
            }

            //使用者是否存在
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "無效的帳號密碼");
                return View(model);
            }

            //密碼檢查
            PasswordVerificationResult status = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password);

            if (user == null || !status.Equals(PasswordVerificationResult.Success))
            {
                ModelState.AddModelError("", "無效的帳號密碼");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                //return RedirectToAction("SendCode");
                ModelState.AddModelError("", "未完成信箱驗證");
                return View(model);
            }



            if (user != null && status.Equals(PasswordVerificationResult.Success))
            {
                await SignInAsync(user, model.RememberMe);

                CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();

                serializeModel.ID = user.Id;
                serializeModel.Name = user.UserName;
                serializeModel.Email = user.Email;
                serializeModel.UserCode = user.UserCode;
                serializeModel.UserType = user.UserType;
                var UserRoles = (from rr in RoleManager.Roles.ToList()
                                 join r1 in user.Roles on rr.Id equals r1.RoleId
                                 select rr.Name).ToList();

                //var r = (from uRoles in user.Roles
                //        join rr in RoleManager.Roles.ToList() on uRoles.RoleId == rr.RoleId).to


                serializeModel.roles = string.Join(",", UserRoles);
                // serializeModel.roles = "Admin";
                var ExpireDateTime = DateTime.Now.AddDays(3);
                if (model.RememberMe)
                {
                    ExpireDateTime = DateTime.Now.AddDays(15);
                }

                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket = null;
                authTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddDays(15), false, userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { Expires = authTicket.Expiration, Path = "/" };
                System.Web.HttpContext.Current.Response.Cookies.Add(faCookie);


                _db.SystemLog.Add(new SystemLog
                {
                    Created = DateTime.Now,
                    Creator = model.Email,
                    IP = IPaddress,
                    LogCode = "Time",
                    LogType = "SignIn",
                    LogDescription = "登入時間",
                    LogValue = DateTime.Now.ToString()
                });
                _db.SaveChanges();

                return RedirectToLocal(returnUrl);
            }
                        
             
             return View(model);

        }


        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="user"></param>
        /// <param name="RememberMe"></param>
        private void SignIn(ApplicationUser user,bool RememberMe)
        {
            
                   CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();

                    serializeModel.ID = user.Id;
                    serializeModel.Name = user.UserName;
                    serializeModel.Email = user.Email;
                    serializeModel.UserCode = user.UserCode;
                    serializeModel.UserType = user.UserType;
                    var UserRoles = (from rr in RoleManager.Roles.ToList()
                                     join r1 in user.Roles on rr.Id equals r1.RoleId
                                     select rr.Name).ToList();
                            
                    //var r = (from uRoles in user.Roles
                    //        join rr in RoleManager.Roles.ToList() on uRoles.RoleId == rr.RoleId).to

                           
                    serializeModel.roles = string.Join(",", UserRoles);
                    // serializeModel.roles = "Admin";
                    var ExpireDateTime = DateTime.Now.AddMinutes(5);
                    if (RememberMe)
                    {
                        ExpireDateTime = DateTime.Now.AddHours(3);
                    }

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = null;
                    authTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddHours(3), false, userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { Expires = authTicket.Expiration, Path = "/" };
                    System.Web.HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

       
        

        
        private string GetRecommendUserCode(string Recommend)
        {
            var RecommendUserCode = string.Empty;
            var RecommendUsers = Account_db.Users.Where(o => o.Recommend == Recommend && o.UserType == "User").ToList();
            if (RecommendUsers.Count < 6)
            {
                RecommendUserCode = Recommend;
            }
            else
            {

                var Recommends = RecommendUsers.Select(o => o.UserCode).ToList();
                RecommendUserCode = GetRecommendForList(Recommends);
            }
            return RecommendUserCode;
        }


        private string GetRecommendForList(List<string> Recommends)
        {
            var RecommendUsers = new List<RecommendUsers>();
            var RecommendList = new List<string>();
            var OverUser = false;
            foreach (var item in Recommends)
            {
                var Users = Account_db.Users.Where(o => o.Recommend == item && o.UserType == "User").ToList();
                foreach (var sub in Users)
                {
                    RecommendList.Add(sub.UserCode);
                }
                if (OverUser == false && Users.Count < 6)
                {                                       
                    OverUser = true;
                }
                RecommendUsers.Add(new RecommendUsers { Count = Users.Count, Recommend = item });
            }

            var R_Recommend = string.Empty;

            if (OverUser)
            {
                var min = RecommendUsers.Min(o => o.Count);
                R_Recommend = RecommendUsers.Where(o => o.Count == min).FirstOrDefault().Recommend;
            }
            else
            {
                R_Recommend = GetRecommendForList(RecommendList);
            }
            return R_Recommend;
        }


        public class RecommendUsers
        {
            public string Recommend { get; set; }
            public int Count { get; set; }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            FormsAuthentication.SignOut();
            AuthenticationManager.SignOut();
            var Recommend = Session["RecommendCode"] == null ? string.Empty : Session["RecommendCode"].ToString();

            ViewBag.Recommend = Recommend;

            var model = new RegisterViewModel();
            model.Email = Session["SingInEmail"] == null ? string.Empty : Session["SingInEmail"].ToString();
            ViewData.Model = model;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var Recommend = Session["RecommendCode"] == null ? string.Empty : Session["RecommendCode"].ToString();
            ViewBag.Recommend = Recommend;
            AddRoles();
            //var Recommend = string.Empty;
            //if (string.IsNullOrEmpty(model.Recommend))
            //{
            //    Recommend = OfficalRecommendCode;
            //}
            
            if (Recommend != OfficalRecommendCode)
            {
                var user = Account_db.Users.Where(o => o.UserCode == Recommend).FirstOrDefault();
                if (user != null && user.UserType.ToUpper() == "HOTEL")
                {
                    ModelState.AddModelError("","旅館推薦人必須為旅客身分");
                    return View();
                }
            }
            model.Recommend = Recommend;
            model.UserCode = new Anything.Helpers.BaseDLL().GetUserCode(model.UserName);
            
            model.UserType = "Hotel";
            //model.Password = model.Password.ToLower();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, UserType = model.UserType, UserCode = model.UserCode,Recommend = model.Recommend };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var roleName = model.UserType;
                    if (!RoleManager.RoleExists(roleName))
                    {
                        var role = new Role(roleName);
                        await RoleManager.CreateAsync(role);
                    }


                    
                    UserManager.AddToRole(user.Id, model.UserType);
                  
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var link = string.Format("信箱驗證連結網址<a href='{0}'>完成驗證</a>", callbackUrl);

                    await UserManager.SendEmailAsync(user.Id, "MYAnything 信箱驗證", link);
                    ViewBag.Link = callbackUrl;
                    return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddRoles()
        {
             var SystemRoles = new string[]{"User","Hotel","Admin","AdManager","Accountant"};
             foreach (var r in SystemRoles)
             {
                 if (!RoleManager.RoleExists(r))
                 {
                     var role = new Role(r);
                     RoleManager.Create(role);
                 }
             }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await UserManager.FindByNameAsync(model.Email);
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ModelState.AddModelError("", "無效的帳號");
                    return View();
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            model.Password = model.Password.ToLower();
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("","使用者不存在");
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                // Don't reveal that the user does not exist
                return View();
            }

            //var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            //if (string.IsNullOrEmpty(model.Code) || !code.Equals(model.Code))
            //{
            //    TempData["SuccessMessage"] = "密碼已變更，下次請使用新密碼";
            //    return Redirect(model.ReturnUrl);
            //}

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    TempData["SuccessMessage"] = "密碼已變更，下次請使用新密碼";
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            TempData["ViewData"] = ViewData;
            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            model.Password = model.Password.ToLower();
            
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return View(model);
            }


            var user = await UserManager.FindByEmailAsync(model.Email);


            if (user == null)
            {
                TempData["ErrorMessage"] = "密碼變更，使用者不存在";
                ModelState.AddModelError("", "使用者不存在");
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                // Don't reveal that the user does not exist
                FormsAuthentication.SignOut();
                AuthenticationManager.SignOut();
                return Redirect("~/Home/Account");
            }
            else
            {
               if(CurrentUser.Id != user.Id){
                   TempData["ErrorMessage"] = "密碼變更，帳號不符";
                    ModelState.AddModelError("", "帳號不符");
                    FormsAuthentication.SignOut();
                    AuthenticationManager.SignOut();
                    return Redirect("~/Home/Account");
               }
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);


            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    TempData["SuccessMessage"] = "密碼已變更，下次請使用新密碼";
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            TempData["ViewData"] = ViewData;
            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            FormsAuthentication.SignOut();
            AuthenticationManager.SignOut();
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId <= 0)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }

        //
        // GET: /Account/ExternalLoginCallback
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            if (loginInfo.Login.LoginProvider == "Facebook")
            {
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(access_token);
                dynamic myInfo = fb.Get("/me?fields=email"); // specify the email field
                loginInfo.Email = myInfo.email;
                //var fb = 
            }

            if (loginInfo.Login.LoginProvider == "Google")
            {
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var email = access_token.Value;
                loginInfo.Email = email;
                //var fb = 
            }

            var user = UserManager.FindByEmail(loginInfo.Email);
            if (user == null)
            {
                Session["SingInEmail"] = loginInfo.Email;
                return RedirectToAction("Register");
            }
            else
            {
                Session["SingInEmail"] = loginInfo.Email;
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            //var result = await SignInManager2.ExternalSignInAsync(loginInfo, isPersistent: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
            //    case SignInStatus.Failure:
            //    default:
            //        // If the user does not have an account, then prompt the user to create an account
            //        ViewBag.ReturnUrl = returnUrl;
            //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            //}
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SocialRegister(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            //return View();
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            var code = new Anything.Helpers.VerificationCode(5);
            var img = code.bytes;
            
            return File(img, "image/jpg");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
           
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}