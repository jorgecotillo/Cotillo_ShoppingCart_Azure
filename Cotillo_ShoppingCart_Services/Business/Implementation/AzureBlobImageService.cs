using Cotillo_ShoppingCart_Services.Business.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class AzureBlobImageService : IImageService
    {
        public string GetImageAsBase64String(string fileName)
        {
            try
            {
                string accountName = ConfigurationManager.AppSettings["StorageName"];
                string accountKey = ConfigurationManager.AppSettings["StorageKey"];
                string containerReference = ConfigurationManager.AppSettings["ImageContainerReference"];

                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer sampleContainer = client.GetContainerReference(containerReference);//This will be the proper container("CartImages");
                CloudBlockBlob blob = sampleContainer.GetBlockBlobReference(fileName);

                using (Stream outputFile = new MemoryStream())
                {
                    blob.DownloadToStream(outputFile);
                    outputFile.Position = 0;
                    byte[] binaryImage = new byte[outputFile.Length];
                    outputFile.Read(binaryImage, 0, (int)outputFile.Length);

                    return Convert.ToBase64String(binaryImage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
