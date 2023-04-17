using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlatform.Tool.StorageAccount
{
    public class StorageBlob: StorageContainer
    {
        public string Name { get; set; }
        public string ContainerName { get; set; }

        public string PublishType { get; set; }

        public string Status { get; set; }

        public string LastModifyTime { get; set; }

        public string Url { get; set; }
    }
}
