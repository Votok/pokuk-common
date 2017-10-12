using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace pokuk_common.services
{
    internal class AzureService
    {
        private GalleryOptions _config;

        private CloudStorageAccount _azureAccount;
        private CloudBlobContainer _container;

        private CloudBlobContainer BlobContainer
        {
            get
            {
                if (_container == null)
                {
                    var serviceClient = _azureAccount.CreateCloudBlobClient();
                    _container = serviceClient.GetContainerReference(_config.AzureContainerName);
                }
                return _container;
            }
        }

        public AzureService(GalleryOptions config)
        {
            _config = config;
            _azureAccount = CloudStorageAccount.Parse(_config.AzureStorageConnectionString);
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
            CloudBlockBlob blob = BlobContainer.GetBlockBlobReference(galleryFile.AzureLikeFileName);
            blob.Properties.ContentType = "image/jpeg";

            System.Console.WriteLine($"Uploading to azure with name: {blob.Name}");

            using (FileStream fs = File.Open(galleryFile.FullFileName, FileMode.Open))
            {
                blob.UploadFromStreamAsync(fs).Wait();
            }
        }

        public void UploadJson(IGallery gallery)
        {
            CloudBlockBlob blob = BlobContainer.GetBlockBlobReference(_config.GalleryJsonName);
            blob.Properties.ContentType = "application/json";

            System.Console.WriteLine($"Uploading gallery json to azure with name: {blob.Name}");

            using (var ms = new MemoryStream())
            {
                var json = JsonConvert.SerializeObject(gallery);
                using (var writer = new StreamWriter(ms))
                {
                    writer.Write(json);
                    writer.Flush();
                    ms.Position = 0;

                    blob.UploadFromStreamAsync(ms).Wait();
                }
            }
        }

        /// Loads gallery json from azure
        public IGallery ReadGallery()
        {
            Gallery model = null;
            CloudBlockBlob blob = BlobContainer.GetBlockBlobReference(_config.GalleryJsonName);

            string text;
            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStreamAsync(memoryStream).Wait();
                text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            model = JsonConvert.DeserializeObject<Gallery>(text);
            return model;
        }

    }
}