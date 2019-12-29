using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Azure.Storage.Blob;

namespace App1.MobileAppService.Services
{
    public sealed class ProfileImageUploader : IProfileImageUploader
    {
        private readonly CloudBlobClient cloudBlobClient;

        public ProfileImageUploader(Microsoft.Azure.Storage.Blob.CloudBlobClient cloudBlobClient)
        {
            this.cloudBlobClient = cloudBlobClient;
        }

        public async Task<Uri> UploadImageAsync(string id, Stream stream)
        {
            var container = cloudBlobClient.GetContainerReference("profile-images");
            if (!await container.ExistsAsync())
            {
                await container.CreateAsync();
            }

            var blob = container.GetBlockBlobReference(id);

            await blob.DeleteIfExistsAsync();

            await blob.UploadFromStreamAsync(stream);

            return blob.Uri;
        }
    }
}
