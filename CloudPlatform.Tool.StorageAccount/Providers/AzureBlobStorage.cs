using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace CloudPlatform.Tool.StorageAccount.Providers;

public class AzureBlobStorage : IBlogStorage
{
    public string Name => nameof(AzureBlobStorage);

    private readonly BlobServiceClient _blobServiceClient;

    private readonly ILogger<AzureBlobStorage> _logger;

    public AzureBlobStorage(ILogger<AzureBlobStorage> logger, AzureBlobConfiguration blobConfiguration)
    {
        _logger = logger;

        _blobServiceClient = new(blobConfiguration.ConnectionString);

        logger.LogInformation($"Created {nameof(AzureBlobStorage)} for account {_blobServiceClient.AccountName} ");
    }

    public async Task DeleteAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("picturecontainer");
        await containerClient.DeleteBlobIfExistsAsync(fileName);
    }

    public async Task<List<StorageBlob>> GetBlobListAsync(string containerName)
    {
        var blobList = new List<StorageBlob>();
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = containerClient.GetBlobsAsync();

            await foreach (var itemBlob in blobs)
            {
                blobList.Add(new StorageBlob
                {
                    Name = itemBlob.Name.Substring(0, itemBlob.Name.Length - 4),
                    Version = itemBlob.Name.Substring(0, itemBlob.Name.Length - 4).Replace('.','-'),
                    ContainerName = containerName,
                    Status = itemBlob.Deleted ? "不可用" : "可用",
                    Url= string.Format("{0}{1}/{2}", _blobServiceClient.Uri, containerName, itemBlob.Name),
                    LastModifyTime = itemBlob.Properties.LastModified.HasValue ? itemBlob.Properties.LastModified.Value.ToString():"" 
                }) ;
            }
        }
        catch (RequestFailedException e)
        {
            _logger.LogWarning($"Blob container not exist.");
        }


        return blobList;
    }

    public async Task<List<StorageContainer>> GetContainerNameListAsync()
    {
        var containersList = new List<StorageContainer>();
        try
        {

            // Call the listing operation and enumerate the result segment.
            var resultSegment =
                _blobServiceClient.GetBlobContainersAsync()
                .AsPages(default);

            await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
            {
                foreach (BlobContainerItem containerItem in containerPage.Values)
                {
                    containersList.Add(new StorageContainer
                    {
                        Name = containerItem.Name,
                        PublishType = containerItem.Properties.PublicAccess?.ToString(),
                        Status = containerItem.IsDeleted == null ? "可用" : (containerItem.IsDeleted.Value ? "可用" : "不可用"),
                        LastModifyTime = containerItem.Properties.LastModified.ToString()
                    });
                }
            }
        }
        catch (RequestFailedException e)
        {
            _logger.LogWarning($"Blob container not exist.");
        }

        return containersList;
    }
}
