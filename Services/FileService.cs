using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace pokuk_common.services
{
    internal class FileService
    {
        private GalleryOptions _config;
        private string _galleryJsonFullName;        

        public FileService(GalleryOptions config)
        {
            _config = config;
            _galleryJsonFullName = Path.Combine(_config.LocalGalleryDirectory, _config.GalleryJsonName);
        }

        /// Returns Gallery from file system.
        public IGallery ReadGallery()
        {
            var gallery = new Gallery();
            var diSource = new DirectoryInfo(_config.LocalGalleryDirectory);

            Console.WriteLine("Creating gallery json from directory: " + diSource.FullName);

            foreach (var di in diSource.GetDirectories().OrderBy(x => x.Name))
            {
                var year = int.Parse(di.Name.Substring(0, 4));

                var galleryYear = gallery.Years.FirstOrDefault(x => x.Year == year);
                if (galleryYear == null)
                {
                    galleryYear = new GalleryYear() { Year = year };
                    gallery.Years.Add(galleryYear);
                }

                var galleryEvent = ReadGalleryEvent(di, galleryYear);
                galleryYear.GalleryEvents.Add(galleryEvent);
            }

            return gallery;
        }

        /// Returns gallery json from file system
        public IGallery ReadGalleryJson()
        {
            IGallery gallery = null;
            using (StreamReader file = File.OpenText(_galleryJsonFullName))
            {
                var serializer = new JsonSerializer();
                gallery = (Gallery)serializer.Deserialize(file, typeof(Gallery));
            }
            return gallery;
        }

        /// Saves gallery json to filesystem
        public void SaveGallery(IGallery gallery)
        {
            using (StreamWriter file = File.CreateText(_galleryJsonFullName))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, gallery);
            }
            Console.WriteLine("Gallery json file saved: " + _galleryJsonFullName);
        }

        /// Reads gallery event folder and returns gallery event
        private GalleryEvent ReadGalleryEvent(DirectoryInfo diEvent, IGalleryYear galleryYear)
        {
            var friendlyNameFile = diEvent.GetFiles("*.txt").FirstOrDefault();

            if (!friendlyNameFile.Exists)
                throw new Exception("Missing txt file with friendly name for: " + diEvent.FullName);

            var galleryEvent = new GalleryEvent(galleryYear.Year.ToString())
            {
                Name = diEvent.Name.Substring(8),
                FriendlyName = friendlyNameFile.Name.Substring(2).Replace(".txt", "")
            };

            foreach (var fi in diEvent.GetFiles().OrderBy(x => x.Name))
            {
                if (fi.Extension != ".jpg")
                    continue;

                var galleryFile = galleryEvent.AddFile(fi, _config.AzureContainerUrl);
            }
            return galleryEvent;
        }

    }
}