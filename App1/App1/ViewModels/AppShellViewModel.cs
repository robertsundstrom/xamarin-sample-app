using App1.Data;
using App1.Services;

using Microsoft.EntityFrameworkCore;

using Xamarin.Forms;

namespace App1.ViewModels
{
    internal class AppShellViewModel : ViewModelBase
    {
        public AppShellViewModel(
            IIdentityService identityService,
            INavigationService navigationService,
            ApplicationDbContext applicationDbContext)
        {
            if (applicationDbContext.Database.IsSqlite())
            {
                applicationDbContext.Database.EnsureCreated();
                applicationDbContext.Database.Migrate();
            }

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
