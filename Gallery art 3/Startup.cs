using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gallery_art_3.Startup))]
namespace Gallery_art_3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
