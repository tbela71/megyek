using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Megyek.Areas.Identity.IdentityHostingStartup))]
namespace Megyek.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}