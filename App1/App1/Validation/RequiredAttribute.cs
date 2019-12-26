using App1.Resources;
using App1.Services;

using Microsoft.Extensions.DependencyInjection;

namespace App1.Validation
{
    public sealed class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        private const string DefaultResourceKey = nameof(AppResources.RequiredInputFieldMessageText);

        public override string FormatErrorMessage(string name)
        {
            string resourceKey = ErrorMessageResourceName ?? DefaultResourceKey;

            string localizedFieldName = App.ServiceProvider
                .GetService<ILocalizationService>()
                .GetString(name);

            return App.ServiceProvider
                .GetService<ILocalizationService>()
                .GetString(resourceKey, localizedFieldName);
        }
    }
}
