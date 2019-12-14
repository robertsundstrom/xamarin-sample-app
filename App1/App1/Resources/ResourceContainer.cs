using System.Globalization;
using System.Resources;

namespace App1.Resources
{
    public class ResourceContainer : IResourceContainer
    {
        public static string ResourceId = "App1.Resources.AppResource"; // The namespace and name of your Resources file
        private readonly CultureInfo _cultureInfo;
        private readonly ResourceManager _resourceManager;

        public ResourceContainer(ResourceManager manager, ILocalize localize)
        {
            _cultureInfo = localize.GetCurrentCultureInfo();
            _resourceManager = manager;
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
