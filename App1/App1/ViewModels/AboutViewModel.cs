using System.Windows.Input;

using App1.Services;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(INavigationService navigationService)
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Launcher.OpenAsync("https://xamarin.com"));
            ShowUserAgreementCommand = new Command(async () => await navigationService.PushModalAsync<UserAgreementViewModel>());
        }

        public ICommand OpenWebCommand { get; }

        public ICommand ShowUserAgreementCommand { get; }

    }
}
