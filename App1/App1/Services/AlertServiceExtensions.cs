using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace App1.Services
{
    static class AlertServiceExtensions
    {
        static AlertServiceExtensions()
        {
            LocalizationService = App.ServiceProvider?.GetService<ILocalizationService>();
        }

        public static ILocalizationService? LocalizationService { get; set; }

        public static Task DisplayAlertOkAsync(this IAlertService alertService, string title, string message)
        {
            return alertService.DisplayAlertAsync(title, message, LocalizationService?.GetString("OkButton") ?? throw new Exception());
        }
    }
}
