namespace CloudPlatform.Tool.StorageAccount;

public interface IBlogStorage
{
    string Name { get; }

    Task DeleteAsync(string fileName);

    Task<List<StorageContainer>> GetContainerNameListAsync();

    Task<List<StorageBlob>> GetBlobListAsync(string containerName);

}