
using Android.App;
using Android.Widget;

namespace App1.Droid
{
    internal class NativeCalls : INativeCalls
    {
        public void OpenToast(string text)
        {
            Toast.MakeText(Application.Context, text, ToastLength.Long).Show();
        }
    }
}