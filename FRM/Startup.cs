using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FRM.Startup))]
namespace FRM
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}