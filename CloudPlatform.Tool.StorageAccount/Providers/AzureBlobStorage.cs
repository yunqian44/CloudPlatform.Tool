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

        _blobServiceClient = new (blobConfiguration.ConnectionString);

        logger.LogInformation($"Created {nameof(AzureBlobStorage)} for account {_blobServiceClient.AccountName} ");
    }

    public async Task DeleteAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("picturecontainer");
        await containerClient.DeleteBlobIfExistsAsync(fileName);
    }

    public async Task<BlobInfo> GetAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("picturecontainer");
        var blobClient = containerClient.GetBlobClient(fileName);
        await using var memoryStream = new MemoryStream();
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException("File extension is empty");
        }

        var existsTask = blobClient.ExistsAsync();
        var downloadTask = blobClient.DownloadToAsync(memoryStream);

        var exists = await existsTask;
        if (!exists)
        {
            _logger.LogWarning($"Blob {fileName} not exist.");

            // Can not throw FileNotFoundException,
            // because hackers may request a large number of 404 images
            // to flood .NET runtime with exceptions and take out the server
            return null;
        }

        await downloadTask;
        var arr = memoryStream.ToArray();

        var fileType = extension.Replace(".", string.Empty);

        return new BlobInfo(memoryStream, fileType);
    }

    public async Task<List<StorageContainer>> GetContainerNameList()
    {
        var containersList = new List<StorageContainer>();
        try
        {
            
            // Call the listing operation and enumerate the result segment.
            var resultSegment =
                _blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, default)
                .AsPages(default);

            await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
            {
                foreach (BlobContainerItem containerItem in containerPage.Values)
                {
                    containersList.Add(new StorageContainer
                    {
                        Name = containerItem.Name,
                        PublishType = containerItem.Properties.PublicAccess?.ToString(),
                        Status=containerItem.IsDeleted.HasValue?"可用":"不可用",
                        LastModifyTime= containerItem.Properties.LastModified.ToString()
                    }) ;
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
