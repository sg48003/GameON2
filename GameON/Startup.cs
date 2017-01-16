using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameON.Startup))]
namespace GameON
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
