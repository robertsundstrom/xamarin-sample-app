using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{

    internal class LoginViewModel : ViewModelBase<LoginViewModelArgs?>
    {
        private readonly INavigationService _navigationService;
        private readonly IIdentityService identityService;
        private readonly INativeCalls nativeCalls;
        private string? email;
        private string? password;
        private bool showLoginNoticeVisible;
        private bool isClean = true;

        public LoginViewModel(
            INavigationService navigationService,
            IIdentityService identityService,
            INativeCalls nativeCalls)
        {
            _navigationService = navigationService;
            this.identityService = identityService;
            this.nativeCalls = nativeCalls;
            LoginCommand = new Command(async () => await ExecuteLoginCommand(), () => CanSubmit);
            NavigateToRegistrationPageCommand = new Command(async () => await navigationService.PushAsync<ViewModels.RegistrationViewModel>());
            NavigateToAboutPageCommand = new Command(async () => await _navigationService.PushAsync<AboutViewModel>());
        }

        public override Task InitializeAsync(LoginViewModelArgs? arg)
        {
            if (arg == null)
            {
                return Task.CompletedTask;
            }

            ShowLoginNoticeVisible = arg.HasSessionExpired;

            return Task.CompletedTask;
        }

        private async Task ExecuteLoginCommand()
        {
            try
            {
                bool response = await identityService.AuthenticateAsync(email!, password!);
                if (response)
                {
                    await _navigationService.PushAsync<AppShellViewModel>();
                }
                else
                {
                    nativeCalls.OpenToast("Invalid email address or password.");
                }
            }
            catch (HttpRequestException exc)
            {
                nativeCalls.OpenToast(exc.Message);
            }
            catch (Exception exc)
            {
                nativeCalls.OpenToast(exc.Message);
            }
        }

        public Command LoginCommand { get; }

        public Command NavigateToRegistrationPageCommand { get; }

        public Command NavigateToAboutPageCommand { get; }

        [Required]
        [EmailAddress]
        public string? Email
        {
            get => email;
            set
            {
                ValidateProperty(value);
                isClean = false;
                SetProperty(ref email, value);
                LoginCommand.ChangeCanExecute();
            }
        }

        [Required]
        public string? Password
        {
            get => password;
            set
            {
                ValidateProperty(value);
                isClean = false;
                SetProperty(ref password, value);
                LoginCommand.ChangeCanExecute();
            }
        }

        protected override void ValidateProperty<T>(T value, [CallerMemberName] string? propertyName = null)
        {
            base.ValidateProperty(value, propertyName);

            OnPropertyChanged(nameof(CanSubmit));
        }

        public bool CanSubmit => !isClean && Validate();

        public bool ShowLoginNoticeVisible
        {
            get => showLoginNoticeVisible;
            set => SetProperty(ref showLoginNoticeVisible, value);
        }
    }
}
