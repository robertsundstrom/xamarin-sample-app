using System;
using System.Threading.Tasks;

using App1.MobileAppService.Client;
using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly IUserClient userClient;

        public Command UpdateUserProfileCommand { get; }
        public Command ChangePasswordCommand { get; }

        private User user;

        public UserProfileViewModel(IUserClient userClient, INavigationService navigationService)
        {
            this.userClient = userClient;

            UpdateUserProfileCommand = new Command(async () => await navigationService.PushAsync<UpdateUserProfileViewModel>(user));
            ChangePasswordCommand = new Command(async () => await navigationService.PushAsync<ChangePasswordViewModel>());
        }

        public User User
        {
            get => user;
            private set
            {
                SetProperty(ref user, value);
            }
        }

        public override async Task InitializeAsync(object? arg)
        {
            try
            {
                User = await userClient.GetUserAsync();
            }
            catch (Exception e)
            {

            }
        }
    }
}
