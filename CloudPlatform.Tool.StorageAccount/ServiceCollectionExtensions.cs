using CloudPlatform.Tool.StorageAccount.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudPlatform.Tool.StorageAccount;

public class BlobStorageOptions
{
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
}

public static class ServiceCollectionExtensions
{
    private static readonly BlobStorageOptions Options = new();

    public static IServiceCollection AddBlobStorage(
        this IServiceCollection services, IConfiguration configuration, Action<BlobStorageOptions> options)
    {
        options(Options);
        var _conn = configuration["AzureBlobStorageConnectionString"];

        if (string.IsNullOrWhiteSpace(_conn?.ToLower()))
        {
            throw new ArgumentNullException("StorageConnection", "conn can not be empty.");
        }


        services.AddAzureStorage(_conn);


        return services;
    }

    private static void AddAzureStorage(this IServiceCollection services, string _conn)
    {
        services.AddSingleton(_ => new AzureBlobConfiguration(_conn))
                .AddSingleton<IBlogStorage, AzureBlobStorage>();
    }
}
