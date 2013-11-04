using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FirebirdWars.Startup))]
namespace FirebirdWars
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
