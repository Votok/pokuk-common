using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public interface IGalleryEvent
    {
        string Name { get; set; }
        string FriendlyName { get; set; }
        List<GalleryFile> Files { get; set; }
        bool IsUploaded { get; }

        IGalleryFile AddFile(FileInfo fi, string azureUrl);
    }
}