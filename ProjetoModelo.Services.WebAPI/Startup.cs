
using Microsoft.Owin;
using Owin;

namespace ProjetoModelo.Services.WebAPI
{
    [assembly: OwinStartup(typeof(Startup))]
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
        }
    }
}