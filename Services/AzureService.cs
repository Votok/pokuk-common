using System;
using System.IO;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace pokuk_common.services
{
    internal class AzureService
    {
        private readonly GalleryOptions _config;
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _container;

        private BlobContainerClient BlobContainer
        {
            get
            {
                if (_container == null)
                {
                    _container = _blobServiceClient.GetBlobContainerClient(_config.AzureContainerName);
                }
                return _container;
            }
        }

        public AzureService(GalleryOptions config)
        {
            _config = config;
            _blobServiceClient = new BlobServiceClient(_config.AzureStorageConnectionString);
        }

        /// Uploads gallery event images file to azure
        public void UploadEvent(IGalleryEvent galleryEvent)
        {
            foreach (var file in galleryEvent.Files)
            {
                Upload(file);
            }
        }

        /// Uploads one gallery image file to azure
        private void Upload(IGalleryFile galleryFile)
        {
            var blobClient = BlobContainer.GetBlobClient(galleryFile.AzureLikeFileName);
            var headers = new BlobHttpHeaders { ContentType = "image/jpeg" };

            Console.WriteLine($"Uploading to azure with name: {blobClient.Name}");

            using (FileStream fs = File.Open(galleryFile.FullFileName, FileMode.Open))
            {
                blobClient.Upload(fs, new BlobUploadOptions { HttpHeaders = headers });
            }
        }

        public void UploadJson(IGallery gallery)
        {
            var blobClient = BlobContainer.GetBlobClient(_config.GalleryJsonName);
            var headers = new BlobHttpHeaders { ContentType = "application/json" };

            Console.WriteLine($"Uploading gallery json to azure with name: {blobClient.Name}");

            var json = JsonConvert.SerializeObject(gallery);
            using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
            blobClient.Upload(ms, new BlobUploadOptions { HttpHeaders = headers });
        }

        /// Loads gallery json from azure
        public IGallery ReadGallery()
        {
            var blobClient = BlobContainer.GetBlobClient(_config.GalleryJsonName);
            var download = blobClient.DownloadContent();
            var text = download.Value.Content.ToString();
            var model = JsonConvert.DeserializeObject<Gallery>(text);
            return model;
        }

    }
}