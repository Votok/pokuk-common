using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public class GalleryFile : IGalleryFile
    {
        public string FileName { get; set; }

        public string FullFileName { get; set; }

        public string AzureLikeFileName { get; set; }

        public string AzureFullUlr { get; set; }

    }
}