using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlatform.Tool.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobConfig(this IServiceCollection services)
        {
            services.AddSingleton<IBlobConfig, BlobConfig>();
            return services;
        }
    }
}
