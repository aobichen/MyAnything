using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anything.Models
{
    public class UserRole2 : IdentityUserRole<int>
    {
    }

    public class UserClaim2 : IdentityUserClaim<int>
    {
    }

    public class UserLogin2 : IdentityUserLogin<int>
    {
    }

    public class Role2 : IdentityRole<int, UserRole2>
    {
        public Role2() { }
        public Role2(string name) { Name = name; }
    }

    public class UserStore2 : UserStore<ApplicationUser2, Role2, int,
        UserLogin2, UserRole2, UserClaim2>
    {
        public UserStore2(ApplicationDbContext2 context)
            : base(context)
        {
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
    }



    public class RoleStore2 : RoleStore<Role, int, UserRole>
    {
        public RoleStore2(ApplicationDbContext2 context)
            : base(context)
        {
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser2 : IdentityUser<int, UserLogin2, UserRole2, UserClaim2>
    {
        public string UserType { get; set; }
        public string UserCode { get; set; }

        public string Recommend { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager2 manager2)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager2.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext2 : IdentityDbContext<ApplicationUser2, Role2, int,
    UserLogin2, UserRole2, UserClaim2>
    {

        public ApplicationDbContext2()
            : base("MyAnythingHotelUsers")
        {
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
            //this.Database.Initialize(true);
        }

        public static ApplicationDbContext2 Create()
        {
            return new ApplicationDbContext2();
        }
    }
}