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

    public async Task<string> InsertAsync(string fileName, byte[] imageBytes)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        _logger.LogInformation($"Uploading {fileName} to Azure Blob Storage.");

        var containerClient = _blobServiceClient.GetBlobContainerClient("picturecontainer");

        var blob = containerClient.GetBlobClient(fileName);

        // Why .NET doesn't have MimeMapping.GetMimeMapping()
        var blobHttpHeader = new BlobHttpHeaders();
        var extension = Path.GetExtension(blob.Uri.AbsoluteUri);
        blobHttpHeader.ContentType = extension.ToLower() switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => blobHttpHeader.ContentType
        };

        await using var fileStream = new MemoryStream(imageBytes);
        var uploadedBlob = await blob.UploadAsync(fileStream, blobHttpHeader);

        _logger.LogInformation($"Uploaded image file '{fileName}' to Azure Blob Storage, ETag '{uploadedBlob.Value.ETag}'. Yeah, the best cloud!");

        return fileName;
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
}
