using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    internal class AppShellViewModel : ViewModelBase
    {
        public AppShellViewModel(IIdentityService identityService, INavigationService navigationService)
        {
            LogOutCommand = new Command(async () =>
            {
                await identityService.LogOutAsync();
                await navigationService.PushAsync<LoginViewModel>(new LoginViewModelArgs
                {
                    HasLoggedOut = true
                });
            });
        }

        public Command LogOutCommand { get; }
    }
}
