using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBannuochoa.Startup))]
namespace WebBannuochoa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
