using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Anything.Models;

namespace Anything.Models
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.


    public class ApplicationUserManager2 : UserManager<ApplicationUser2, int>
    {
        public ApplicationUserManager2(IUserStore<ApplicationUser2, int> store)
            : base(store)
        {
        }


        public static ApplicationUserManager2 Create(IdentityFactoryOptions<ApplicationUserManager2> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager2(new UserStore2(context.Get<ApplicationDbContext2>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser2, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser2, int>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser2, int>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService2();
            //manager.SmsService = new SmsService2();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser2, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager2 : RoleManager<Role2, int>
    {
        public ApplicationRoleManager2(IRoleStore<Role2, int> roleStore)
            : base(roleStore)
        {

        }

        public static ApplicationRoleManager2 Create(IdentityFactoryOptions<ApplicationRoleManager2> options, IOwinContext context)
        {
            return new ApplicationRoleManager2(new RoleStore<Role2, int, UserRole2>(context.Get<ApplicationDbContext2>()));
        }
    }

    public class EmailService2 : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {

            var SendMailFromWebsite = ConfigurationManager.AppSettings["SendMailFromWebsite"].ToString();
            if (SendMailFromWebsite == "true")
            {
                var From = ConfigurationManager.AppSettings["From"];
                var Password = ConfigurationManager.AppSettings["SmtpPassword"];
                var SmtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];
                var Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                var SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];

                SmtpClient smtpClient = new SmtpClient(SmtpServer, Port);


                smtpClient.Credentials = new System.Net.NetworkCredential(From, Password);
                //smtpClient.UseDefaultCredentials = true;              
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.Body = message.Body;
                mail.Subject = message.Subject;
                mail.IsBodyHtml = true;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Priority = MailPriority.Normal;
                mail.From = new MailAddress(From, SmtpUserName);
                mail.To.Add(new MailAddress(message.Destination));

                smtpClient.Send(mail);
                smtpClient = null;
                mail.Dispose();


            }

            return Task.FromResult(0);
        }
    }

    //public class SmsService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your sms service here to send a text message.
    //        return Task.FromResult(0);
    //    }
    //}

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer2 : DropCreateDatabaseIfModelChanges<ApplicationDbContext2>
    {
        protected override void Seed(ApplicationDbContext2 context)
        {
            InitializeIdentityForEF2(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF2(ApplicationDbContext2 db)
        {
            db.Configuration.LazyLoadingEnabled = true;
            var userManager2 = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager2>();
            var roleManager2 = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager2>();
            const string name = "admin@example.com";
            const string password = "Aa@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager2.FindByName(roleName);
            //var role = new Role(roleName);
            if (role == null)
            {

                var roleresult = roleManager2.Create(role);
            }

            var user = userManager2.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser2 { UserName = name, Email = name };
                var result = userManager2.Create(user, password);
                result = userManager2.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager2.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager2.AddToRole(user.Id, role.Name);
            }
        }
    }

    public class ApplicationSignInManager2 : SignInManager<ApplicationUser2, int>
    {
        public ApplicationSignInManager2(ApplicationUserManager2 userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser2 user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager2)UserManager);
        }

        public static ApplicationSignInManager2 Create(IdentityFactoryOptions<ApplicationSignInManager2> options, IOwinContext context)
        {
            return new ApplicationSignInManager2(context.GetUserManager<ApplicationUserManager2>(), context.Authentication);
        }
    }
}