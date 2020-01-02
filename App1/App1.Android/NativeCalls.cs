
using System.Threading.Tasks;

using Android.App;
using Android.Widget;

namespace App1.Droid
{
    internal class NativeCalls : INativeCalls
    {
        public Task<string> OpenInputToast(string title, string text, string placeholder, string accept = "OK", string cancel = "Cancel")
        {
            return App.Current.MainPage.DisplayPromptAsync(title, text, accept, cancel, placeholder);
        }

        public void OpenToast(string title, string text, string accept = "OK")
        {
            Toast.MakeText(Application.Context, text, ToastLength.Long).Show();
        }
    }
}
