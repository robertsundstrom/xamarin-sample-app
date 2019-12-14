using System;
using System.Diagnostics;

using App1.Services;

using Microsoft.Extensions.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Markup
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private static readonly bool _initialized = false;
        private static readonly ILocalizationService _localizationService = null;

        static TranslateExtension()
        {
            _initialized = true;
            _localizationService = App.ServiceProvider.GetRequiredService<ILocalizationService>();
        }

        public TranslateExtension()
        {

        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!_initialized)
            {
                throw new NullReferenceException($"{nameof(TranslateExtension)} can not be called as it was not initialized. You must call Init() first.");
            }

            if (Text == null)
            {
                return "";
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
