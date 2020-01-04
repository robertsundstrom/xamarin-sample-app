using System.Threading.Tasks;

namespace App1.Services
{
    sealed class AlertService : IAlertService
    {
        public Task DisplayAlertAsync(string title, string message, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
