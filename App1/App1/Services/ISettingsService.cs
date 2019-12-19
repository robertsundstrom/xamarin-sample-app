using System.Threading.Tasks;

namespace App1.Services
{
    public interface ISettingsService
    {
        string? AuthAccessToken { get; set; }
        string IdAppServiceEndpointBase { get; set; }

        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
    }
}
