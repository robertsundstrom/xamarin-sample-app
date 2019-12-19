using App1.Resources;

namespace App1.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IResourceContainer resourceContainer;

        public LocalizationService(IResourceContainer resourceContainer)
        {
            this.resourceContainer = resourceContainer;
        }

        public string GetString(string key)
        {
            return resourceContainer.GetString(key);
        }

        public string GetString(string key, params object[] args)
        {
            return string.Format(GetString(key), args);
        }
    }
}
