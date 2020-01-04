using System.Threading.Tasks;

namespace App1.Services
{
    sealed class AlertService : IAlertService
    {
        public Task DisplayAlertAsync(string title, string message, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = null, int maxLength = -1)
        {
            return App.Current.MainPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength);
        }
    }
}
