using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App1.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfigurationRoot configuration;

        public UserController(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        //// GET: api/User/5
        //[HttpGet]
        //public string GetProfilePicture(int id)
        //{
        //    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        //    return "value";
        //}

        //// POST: api/User
        //[HttpPost]
        //public async Task UpdateProfilePicture(IFormFile file)
        //{
        //    using (var stream = file.OpenReadStream())
        //    {
        //        await UploadFileToStorage(stream);
        //    }
        //}

        //public async Task<string> UploadFileToStorage(Stream fileStream)
        //{
        //    var blobStorageSection = configuration.GetSection("BlobStorage");

        //    // Create storagecredentials object by reading the values from the configuration (appsettings.json)
        //    StorageCredentials storageCredentials = new StorageCredentials(blobStorageSection["AccountName"], blobStorageSection["AccountKey"]);

        //    // Create cloudstorage account by passing the storagecredentials
        //    CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

        //    // Create the blob client.
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
        //    CloudBlobContainer container = blobClient.GetContainerReference(blobStorageSection["ImageContainer"]);

        //    var blobName = Guid.NewGuid().ToString();

        //    // Get the reference to the block blob from the container
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

        //    // Upload the file
        //    await blockBlob.UploadFromStreamAsync(fileStream);

        //    return blobName;
        //}
    }
}
