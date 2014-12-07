using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebCRM.Startup))]
namespace WebCRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
