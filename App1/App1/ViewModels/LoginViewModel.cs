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
        private readonly IAlertService alertService;
        private string? email;
        private string? password;
        private bool showLoginNoticeVisible;
        private bool isPristine = false;

        public LoginViewModel(
            INavigationService navigationService,
            IIdentityService identityService,
            ILocalizationService localizationService,
            IAlertService alertService)
        {
            _navigationService = navigationService;
            this.identityService = identityService;
            this.localizationService = localizationService;
            this.alertService = alertService;
            LoginCommand = new Command(async () => await ExecuteLoginCommand(), () => !IsPristine && !IsBusy);
            NavigateToRegistrationPageCommand = new Command(async () => await navigationService.PushAsync<ViewModels.RegistrationViewModel>(), () => !IsBusy);
            NavigateToAboutPageCommand = new Command(async () => await _navigationService.PushAsync<AboutViewModel>(), () => !IsBusy);
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
                await alertService.DisplayAlertAsync(string.Empty, localizationService.GetString(nameof(AppResources.CheckFieldsMessage)), "OK");
                return;
            }

            IsBusy = true;

            try
            {
                bool response = await identityService.AuthenticateAsync(email!, password!);
                if (response)
                {
                    await _navigationService.PushAsync<AppShellViewModel>();
                }
                else
                {
                    await alertService.DisplayAlertAsync(string.Empty, "Invalid email address or password.", "OK");
                }
            }
            catch (HttpRequestException exc)
            {
                await alertService.DisplayAlertAsync(string.Empty, exc.Message, "OK");
            }
            catch (Exception exc)
            {
                await alertService.DisplayAlertAsync(string.Empty, exc.Message, "OK");
            }
            finally
            {
                IsBusy = false;
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
