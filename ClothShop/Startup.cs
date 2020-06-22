using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClothShop.Startup))]
namespace ClothShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
