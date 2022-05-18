using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialIt.Startup))]
namespace SocialIt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
