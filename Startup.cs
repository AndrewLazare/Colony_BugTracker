using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(COLONY_THE_BUGTRACKER.Startup))]
namespace COLONY_THE_BUGTRACKER
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
