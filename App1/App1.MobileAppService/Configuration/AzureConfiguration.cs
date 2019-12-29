namespace App1.MobileAppService.Configuration
{
    public class AzureConfiguration
    {
        public BlobStorageConfiguration BlobStorage { get; set; }
    }

    public class BlobStorageConfiguration
    {
        public string ConnectionString { get; set; }
    }
}
