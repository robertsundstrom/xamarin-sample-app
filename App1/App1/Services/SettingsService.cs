using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App1.Services
{
    public sealed class SettingsService : ISettingsService
    {
        private const string AccessToken = "access_token";
        private const string IdAppServiceBase = "url_app_service";

        private readonly string AccessTokenDefault = string.Empty;

        private readonly string UrlAppServiceBaseDefault = string.Empty;

        public string? AuthAccessToken
        {
            get => GetValueOrDefault(AccessToken, AccessTokenDefault);
            set => AddOrUpdateValue(AccessToken, value!);
        }

        public string IdAppServiceEndpointBase
        {
            get => GetValueOrDefault(IdAppServiceBase, UrlAppServiceBaseDefault);
            set => AddOrUpdateValue(IdAppServiceBase, value);
        }

        #region Public Methods

        public Task AddOrUpdateValue(string key, bool value)
        {
            return AddOrUpdateValueInternal(key, value);
        }

        public Task AddOrUpdateValue(string key, string value)
        {
            return AddOrUpdateValueInternal(key, value);
        }

        public bool GetValueOrDefault(string key, bool defaultValue)
        {
            return GetValueOrDefaultInternal(key, defaultValue);
        }

        public string GetValueOrDefault(string key, string defaultValue)
        {
            return GetValueOrDefaultInternal(key, defaultValue);
        }

        #endregion

        #region Internal Implementation

        private async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        private T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object? value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        private async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }

        #endregion
    }
}
