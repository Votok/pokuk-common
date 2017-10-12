using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public class GalleryOptions
    {
        public string LocalGalleryDirectory { get; set; } 
        public string GalleryJsonName { get; set; } = "_gallery.json";
        public string AzureContainerName { get; set; } 
        public string AzureContainerUrl { get; set; } 
        public string AzureStorageConnectionString { get; set; } 
    }
}