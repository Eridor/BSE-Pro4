using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BSE_Pro4.Startup))]
namespace BSE_Pro4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
