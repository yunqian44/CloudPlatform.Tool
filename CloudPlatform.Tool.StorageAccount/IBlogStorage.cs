namespace CloudPlatform.Tool.StorageAccount;

public interface IBlogStorage
{
    string Name { get; }

    Task<string> InsertAsync(string fileName, byte[] imageBytes);

    Task<BlobInfo> GetAsync(string fileName);

    Task DeleteAsync(string fileName);
}