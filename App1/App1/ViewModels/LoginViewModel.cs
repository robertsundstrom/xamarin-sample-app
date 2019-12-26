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
        }

        public override Task InitializeAsync(LoginViewModelArgs? arg)
        {
            if (arg == null)
            {
                return Task.CompletedTask;
            }

            return Task.Run(() =>
            {
                ShowLoginNoticeVisible = arg.HasSessionExpired;
            });
        }

        private async Task ExecuteLoginCommand()
        {
            try
            {
                var response = await identityService.AuthenticateAsync(email!, password!);
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
