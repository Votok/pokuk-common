using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pokuk_common.services;

namespace pokuk_common
{
    public class GalleryProvider
    {
        private AzureService _azureService;
        private FileService _fileService;

        public GalleryProvider(GalleryOptions config)
        {
            _azureService = new AzureService(config);
            _fileService = new FileService(config);
        }

        public IGallery ReadAzureGalleryJson() {
            return _azureService.ReadGallery();
        }

        /// Adds new gallery years, events, images to azure if they exist locally.
        public void AddNewEventsToAzure()
        {
            var localGallery = _fileService.ReadGallery();
            _fileService.SaveGallery(localGallery);

            var azureGallery = _azureService.ReadGallery();

            Console.WriteLine("Searching for new gallery events...");

            bool isChanged = false;
            foreach (var localYear in localGallery.Years)
            {
                if (TryAddEvents(localGallery, azureGallery, localYear))
                    isChanged = true;
            }

            if (isChanged)
                _azureService.UploadJson(azureGallery);
        }

        private bool TryAddEvents(IGallery localGallery, IGallery azureGallery, GalleryYear localYear)
        {
            bool result = false;

            Action<GalleryEvent> uploadEvent = (galleryEvent) => {
                Console.WriteLine("Uploading new gallery event: " + galleryEvent.Name);
                _azureService.UploadEvent(galleryEvent);
            };

            var foundAzureYear = azureGallery.Years.FirstOrDefault(x => x.Year == localYear.Year);
            if (foundAzureYear != null)
            {
                // Year already exists in Azure - add new local events if any
                foreach (var galleryEvent in localYear.GalleryEvents)
                {
                    var foundAzureEvent = foundAzureYear.GalleryEvents.FirstOrDefault(x => x.Name == galleryEvent.Name);
                    if (foundAzureEvent == null)
                    {
                        foundAzureYear.GalleryEvents.Add(galleryEvent);
                        uploadEvent(galleryEvent);
                        result = true;
                    }
                }
            }
            else
            {
                // Add new year with events and upload event photos to azure
                azureGallery.Years.Add(localYear);
                localYear.GalleryEvents.ForEach(x => uploadEvent(x));
                result = true;
            }
            return result;
        }
    }
}