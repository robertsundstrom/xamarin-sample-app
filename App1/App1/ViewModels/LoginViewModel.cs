using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using App1.Resources;
using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{

    internal class LoginViewModel : ViewModelBase<LoginViewModelArgs?>
    {
        private readonly INavigationService _navigationService;
        private readonly IIdentityService identityService;
        private readonly ILocalizationService localizationService;
        private readonly INativeCalls nativeCalls;
        private string? email;
        private string? password;
        private bool showLoginNoticeVisible;
        private bool isPristine = false;

        public LoginViewModel(
            INavigationService navigationService,
            IIdentityService identityService,
            ILocalizationService localizationService,
            INativeCalls nativeCalls)
        {
            _navigationService = navigationService;
            this.identityService = identityService;
            this.localizationService = localizationService;
            this.nativeCalls = nativeCalls;
            LoginCommand = new Command(async () => await ExecuteLoginCommand(), () => !IsPristine);
            NavigateToRegistrationPageCommand = new Command(async () => await navigationService.PushAsync<ViewModels.RegistrationViewModel>());
            NavigateToAboutPageCommand = new Command(async () => await _navigationService.PushAsync<AboutViewModel>());
        }

        public override async Task InitializeAsync(LoginViewModelArgs? arg)
        {
            ShowLoginNoticeVisible = false;

            if (arg != null)
            {
                ShowLoginNoticeVisible = arg.HasSessionExpired;

                if (ShowLoginNoticeVisible)
                {
                    await identityService.LogOutAsync();
                }
            }
        }

        private async Task ExecuteLoginCommand()
        {
            if (!Validate())
            {
                nativeCalls.OpenToast(string.Empty, localizationService.GetString(nameof(AppResources.CheckFieldsMessage)));
                return;
            }

            try
            {
                bool response = await identityService.AuthenticateAsync(email!, password!);
                if (response)
                {
                    await _navigationService.PushAsync<AppShellViewModel>();
                }
                else
                {
                    nativeCalls.OpenToast(string.Empty, "Invalid email address or password.");
                }
            }
            catch (HttpRequestException exc)
            {
                nativeCalls.OpenToast(string.Empty, exc.Message);
            }
            catch (Exception exc)
            {
                nativeCalls.OpenToast(string.Empty, exc.Message);
            }
        }

        public Command LoginCommand { get; }

        public Command NavigateToRegistrationPageCommand { get; }

        public Command NavigateToAboutPageCommand { get; }

        [Required]
        [EmailAddress(ErrorMessageResourceName = nameof(AppResources.EmailIsInvalid), ErrorMessageResourceType = typeof(AppResources))]
        public string? Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
            }
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }

        public bool ShowLoginNoticeVisible
        {
            get => showLoginNoticeVisible;
            set => SetProperty(ref showLoginNoticeVisible, value);
        }

        public bool IsPristine
        {
            get => isPristine;
            protected set => base.SetProperty(ref isPristine, value);
        }

        protected override bool SetProperty<T>(ref T backingStore, T value,
                [CallerMemberName] string propertyName = "",
                Action? onChanged = null)
        {
            if (ValidateProperty(value, false, propertyName))
            {
                RemoveErrors(propertyName);
            }
            IsPristine = false;
            bool result = base.SetProperty(ref backingStore, value, propertyName, onChanged);
            LoginCommand.ChangeCanExecute();
            return result;
        }
    }
}
