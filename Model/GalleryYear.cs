using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pokuk_common
{
    public class GalleryYear : IGalleryYear
    {
        public GalleryYear()
        {
            GalleryEvents = new List<GalleryEvent>();
        }

        public int Year { get; set; }

        public List<GalleryEvent> GalleryEvents { get; set; }
    }
}