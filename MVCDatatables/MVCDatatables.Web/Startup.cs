using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCDatatables.Web.Startup))]
namespace MVCDatatables.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
