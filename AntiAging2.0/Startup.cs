using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AntiAging2._0.Startup))]
namespace AntiAging2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
