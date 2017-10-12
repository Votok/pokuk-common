using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public interface IGallery
    {
        List<GalleryYear> Years { get; set; }
    }
}