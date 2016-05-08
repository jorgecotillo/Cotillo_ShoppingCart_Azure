using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Cotillo_ShoppingCart_Azure.Startup))]


namespace Cotillo_ShoppingCart_Azure
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}
