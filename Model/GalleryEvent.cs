using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public class GalleryEvent : IGalleryEvent
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public List<GalleryFile> Files { get; set; }

        private string _year;

        public bool IsUploaded
        {
            get
            {
                return Files.All(x => x.AzureFullUlr != null);
            }
        }

        public GalleryEvent(string year)
        {
            Files = new List<GalleryFile>();
            _year = year;
        }

        public IGalleryFile AddFile(FileInfo fi, string azureUrl)
        {
            var file = new GalleryFile();
            file.FileName = fi.Name;
            file.FullFileName = fi.FullName;
            file.AzureLikeFileName = $"{_year}_{Name}_{fi.Name}";
            file.AzureFullUlr = azureUrl + file.AzureLikeFileName;

            Files.Add(file);

            return file;
        }
    }
}