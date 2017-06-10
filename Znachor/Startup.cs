using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Znachor.Startup))]
namespace Znachor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
