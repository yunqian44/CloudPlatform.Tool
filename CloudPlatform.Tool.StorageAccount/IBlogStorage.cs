namespace CloudPlatform.Tool.StorageAccount;

public interface IBlogStorage
{
    string Name { get; }

    Task<BlobInfo> GetAsync(string fileName);

    Task DeleteAsync(string fileName);


    Task<List<StorageContainer>> GetContainerNameList();

}