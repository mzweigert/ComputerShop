using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComputerShop.Startup))]
namespace ComputerShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
