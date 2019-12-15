using System;
using System.Diagnostics;

using App1.Services;

using Microsoft.Extensions.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Markup
{
    [ContentProperty("Text")]
    public class LocalizedStringExtension : IMarkupExtension
    {
        private static readonly ILocalizationService _localizationService;

        static LocalizedStringExtension()
        {
            _localizationService = App.ServiceProvider.GetRequiredService<ILocalizationService>();
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public LocalizedStringExtension()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            // Default constructor is required by Xamarin.Forms
        }

        public string? Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return string.Empty;
            }

            var translation = _localizationService.GetString(Text);

            if (translation == null)
            {
                Debug.WriteLine(string.Format("Key '{0}' was not found in resources.", Text)); // I want to know about this during debugging

                translation = Text; // Returns the key, which gets displayed to the user as a last resort effort to display something meaningful
            }

            return translation;
        }
    }
}
