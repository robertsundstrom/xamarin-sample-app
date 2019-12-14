
using Foundation;

using System.Globalization;
using System.Threading;

namespace App1.iOS
{
    internal class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            string netLanguage = "en";
            string prefLanguageOnly = "en";

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                string pref = NSLocale.PreferredLanguages[0];
                prefLanguageOnly = pref.Substring(0, 2);

                if (prefLanguageOnly == "pt")
                {
                    if (pref == "pt")
                    {
                        pref = "pt-BR"; // Brazilian
                    }
                    else
                    {
                        pref = "pt-PT"; // Portuguese
                    }
                }

                netLanguage = pref.Replace("_", "-");
            }

            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(netLanguage);
            }
            catch
            {
                // Fallback to first two characters, e.g. "en"
                cultureInfo = new CultureInfo(prefLanguageOnly);
            }

            return cultureInfo;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}