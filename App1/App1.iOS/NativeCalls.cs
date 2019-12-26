using UIKit;

namespace App1.iOS
{
    internal class NativeCalls : INativeCalls
    {
        public void OpenToast(string text)
        {
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var okAlert = UIAlertController.Create(string.Empty, text, UIAlertControllerStyle.Alert);
            okAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            vc.PresentViewController(okAlert, true, null);
        }
    }
}