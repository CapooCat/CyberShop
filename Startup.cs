using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CyberShop.Startup))]
namespace CyberShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
