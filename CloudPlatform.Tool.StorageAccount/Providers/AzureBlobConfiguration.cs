using System;
namespace CloudPlatform.Tool.StorageAccount.Providers;

public class AzureBlobConfiguration
{
    public string ConnectionString { get; }

    public AzureBlobConfiguration(string connectionString)
    {
        ConnectionString = connectionString;
    }
}
