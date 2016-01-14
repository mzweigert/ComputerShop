using ComputerShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComputerShop.Managers
{
    public class ApplicationRoleManager : RoleManager<Role, long>
    {
        public ApplicationRoleManager(IRoleStore<Role, long> roleStore)
        : base(roleStore)
        { }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var manager = new ApplicationRoleManager(new RoleStore<Role, long, UserRole>(context.Get<ApplicationDbContext>()));
            return manager;
        }
    }
}