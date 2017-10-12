using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public class Gallery : IGallery
    {
        public List<GalleryYear> Years { get; set; }

        public Gallery()
        {
            Years = new List<GalleryYear>();
        }
    }
}