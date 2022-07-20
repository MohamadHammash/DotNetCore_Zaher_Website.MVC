using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Zaher.Ui.Areas.Identity.IdentityHostingStartup))]

namespace Zaher.Ui.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
