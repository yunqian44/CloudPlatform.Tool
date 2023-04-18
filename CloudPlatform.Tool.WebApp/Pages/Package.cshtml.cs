using CloudPlatform.Tool.StorageAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace CloudPlatform.Tool.WebApp.Pages
{
    public class PackageModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IReadOnlyList<StorageBlob> StorageBlobs { get; set; }

        private readonly IBlogStorage _blogStorage;

        public string ContainerName { get; set; }

        public PackageModel(IBlogStorage blobStorage)
        {
            _blogStorage = blobStorage;
        }

        public async Task OnGetAsync(string containerName)
        {
            ContainerName = containerName;
            StorageBlobs = await _blogStorage.GetBlobListAsync(containerName);
            //return Page();
        }
    }
}
