using System.Globalization;
using System.Reflection;
using System.Resources;

namespace App1.Resources
{
    public class ResourceContainer : IResourceContainer
    {
        private readonly CultureInfo _cultureInfo;
        private readonly ResourceManager _resourceManager;

        public ResourceContainer(ILocalize localize)
        {
            _cultureInfo = localize.GetCurrentCultureInfo();

            var resourceType = typeof(AppResources).GetTypeInfo();
            var assembly = resourceType.Assembly;
            string resourceId = resourceType.FullName;

            _resourceManager = new ResourceManager(resourceId, assembly);
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key, _cultureInfo);
        }

        public string GetString(string key, params object[] args)
        {
            return string.Format(GetString(key), args);
        }
    }
}
