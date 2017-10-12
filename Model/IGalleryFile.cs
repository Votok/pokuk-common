using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public interface IGalleryFile
    {
        string FileName { get; set; }
        string FullFileName { get; set; }
        string AzureLikeFileName { get; set; }
        string AzureFullUlr { get; set; }
    }
}