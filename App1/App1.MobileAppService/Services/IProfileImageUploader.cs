using System;
using System.IO;
using System.Threading.Tasks;

namespace App1.MobileAppService.Services
{
    public interface IProfileImageUploader
    {
        Task<Uri> UploadImageAsync(string id, Stream stream);
    }
}
