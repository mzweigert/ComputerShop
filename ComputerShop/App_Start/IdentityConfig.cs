using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ComputerShop.Models;
using ComputerShop.Managers;

namespace ComputerShop
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, long>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext
                .Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            var roleManager = HttpContext.Current
                .GetOwinContext()
                .Get<ApplicationRoleManager>();

  
            const string name = "admin";
            const string password = "admin";
            const string roleAdmin = "Admin";
            const string roleUser = "User";

            //Create Role Admin if it does not exist
            var roleA = roleManager.FindByName(roleAdmin);
            if (roleA == null)
            {

                roleA = new Role();
                roleA.Name = roleAdmin;
                var roleresult = roleManager.Create(roleA);
            }

            //Create Role User if it does not exist
            var roleU = roleManager.FindByName(roleUser);
            if (roleU == null)
            {

                roleU = new Role();
                roleU.Name = roleUser;
                var roleresult = roleManager.Create(roleU);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = "computershop@admin.com" };
                var result = userManager.Create(user, password);
                if(result.Succeeded)
                {
                    result = userManager.SetLockoutEnabled(user.Id, false);

                    // Add user admin to Role Admin if not already added
                    var rolesForUser = userManager.GetRoles(user.Id);
                    if (!rolesForUser.Contains(roleA.Name))
                    {
                        userManager.AddToRole(user.Id, roleA.Name);
                    }
                }
                
            }

            
        }
    }
}
