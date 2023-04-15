using CloudPlatform.Tool.StorageAccount;
using CloudPlatform.Tool.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudPlatform.Tool.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IReadOnlyList<Package> Packages { get; set; }

        private readonly IBlogStorage _blogStorage;

        public IndexModel(IBlogStorage blobStorage)
        {
            _blogStorage = blobStorage;
        }

        public async Task OnGet()
        {
            var imageInfo = await _blogStorage.GetAsync("");
        }
    }
}