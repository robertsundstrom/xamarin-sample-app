using System.Threading.Tasks;

using UIKit;

namespace App1.iOS
{
    internal class NativeCalls : INativeCalls
    {
        public Task OpenToast(string title, string text, string accept = "OK")
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var okAlert = UIAlertController.Create(title, text, UIAlertControllerStyle.Alert);
            okAlert.AddAction(UIAlertAction.Create(accept, UIAlertActionStyle.Default, (a) => taskCompletionSource.SetResult(null)));
            vc.PresentViewController(okAlert, true, null);

            return taskCompletionSource.Task;
        }

        public Task<string> OpenInputToast(string title, string text, string placeholder, string accept = "OK", string cancel = "Cancel")
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            UITextField input = null;
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var okAlert = UIAlertController.Create(title, text, UIAlertControllerStyle.Alert);
            okAlert.AddTextField((edit) =>
            {
                edit.Placeholder = placeholder;
                input = edit;
            });
            okAlert.AddAction(UIAlertAction.Create(accept, UIAlertActionStyle.Default, (a) => taskCompletionSource.SetResult(input.Text)));
            okAlert.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Default, (a) => taskCompletionSource.SetResult(null)));
            vc.PresentViewController(okAlert, true, null);

            return taskCompletionSource.Task;
        }
    }
}
