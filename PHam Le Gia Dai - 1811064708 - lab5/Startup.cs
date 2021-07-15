using Microsoft.Owin;
using Owin;
using PHam_Le_Gia_Dai___1811064708___lab5;

[assembly: OwinStartupAttribute(typeof(Startup))]

namespace PHam_Le_Gia_Dai___1811064708___lab5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}