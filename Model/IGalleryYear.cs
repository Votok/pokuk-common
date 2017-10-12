using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public interface IGalleryYear
    {
        int Year { get; set; }
        List<GalleryEvent> GalleryEvents { get; set; }
    }
}