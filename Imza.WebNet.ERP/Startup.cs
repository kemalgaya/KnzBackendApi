using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Imza.WebNet.ERP.Startup))]
namespace Imza.WebNet.ERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
