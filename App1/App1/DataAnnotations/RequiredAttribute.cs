using App1.Services;

using Microsoft.Extensions.DependencyInjection;

namespace App1.DataAnnotations
{
    public sealed class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        private const string DefaultResourceKey = "RequiredInputFieldXMessageText";

        public override string FormatErrorMessage(string name)
        {
            var resourceKey = ErrorMessageResourceName ?? DefaultResourceKey;

            return App.ServiceProvider
                .GetService<ILocalizationService>()
                .GetString(resourceKey, name);
        }
    }
}
