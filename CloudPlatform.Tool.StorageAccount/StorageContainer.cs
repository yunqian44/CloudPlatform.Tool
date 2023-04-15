using Azure.Storage.Blobs.Models;

namespace CloudPlatform.Tool.StorageAccount;

public class StorageContainer
{
    public string Name { get; set; }

    public string PublishType { get; set; }

    public string Status { get; set; }

    public string LastModifyTime { get; set; }
}