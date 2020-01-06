using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace App1.Services
{
    static class AlertServiceExtensions
    {
        private static readonly ILocalizationService localizationService;

        static AlertServiceExtensions()
        {
            localizationService = App.ServiceProvider.GetService<ILocalizationService>();
        }

        public static Task DisplayAlertOkAsync(this IAlertService alertService, string title, string message)
        {
            return alertService.DisplayAlertAsync(title, message, localizationService.GetString("OkButton"));
        }
    }
}
