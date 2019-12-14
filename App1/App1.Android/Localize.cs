using System.Globalization;
using System.Threading;

namespace App1.Droid
{
    internal class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            Java.Util.Locale androidLocale = Java.Util.Locale.Default;
            string netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return new CultureInfo(netLanguage);
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}