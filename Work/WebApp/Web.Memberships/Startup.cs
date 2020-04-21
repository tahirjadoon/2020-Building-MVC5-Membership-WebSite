using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web.Memberships.Startup))]
namespace Web.Memberships
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
