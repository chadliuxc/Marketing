using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DigitalMarketing.Web.Startup))]
namespace DigitalMarketing.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
