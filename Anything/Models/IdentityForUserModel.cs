using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anything.Models
{
    //public class UserRole : IdentityUserRole<int>
    //{
    //}

    //public class UserClaim : IdentityUserClaim<int>
    //{
    //}

    //public class UserLogin : IdentityUserLogin<int>
    //{
    //}

    //public class Role : IdentityRole<int, UserRole>
    //{
    //    public Role() { }
    //    public Role(string name) { Name = name; }
    //}

    public class UserForStore : UserStore<ApplicationUser, Role, int,
        UserLogin, UserRole, UserClaim>
    {
        public UserForStore(ApplicationForUserDbContext context)
            : base(context)
        {
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
    }

    public class RoleForStore : RoleStore<Role, int, UserRole>
    {
        public RoleForStore(ApplicationForUserDbContext context)
            : base(context)
        {
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationForUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public string UserType { get; set; }
        public string UserCode { get; set; }

        public string Recommend { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationForUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationForUserDbContext : IdentityDbContext<ApplicationUser, Role, int,
    UserLogin, UserRole, UserClaim>
    {
        public ApplicationForUserDbContext()
            : base("MyAnythingEntities")
        {
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
            //this.Database.Initialize(true);
        }

        public static ApplicationForUserDbContext Create()
        {
            return new ApplicationForUserDbContext();
        }
    }
}