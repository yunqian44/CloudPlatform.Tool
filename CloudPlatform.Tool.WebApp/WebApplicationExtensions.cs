using CloudPlatform.Tool.Configuration;
using CloudPlatform.Tool.StorageAccount;
using Microsoft.Extensions.Options;

namespace CloudPlatform.Tool.WebApp
{
    public static class WebApplicationExtensions
    {
        public static async Task InitStartUp(this WebApplication app,
            IConfiguration configuration)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            // load configurations into singleton
            var section = configuration.GetSection(nameof(GeneralSettings));
            var settings = section.Get<GeneralSettings>();
            var bc = app.Services.GetRequiredService<IBlobConfig>();
            bc.LoadFromConfig(settings.ToJson());
        }
    }
}
