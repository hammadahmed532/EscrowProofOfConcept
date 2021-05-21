using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EscrowProofOfConcept.Startup))]
namespace EscrowProofOfConcept
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
