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
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;
        private readonly INavigationService _navigationService;
        private readonly ILocalizationService localizationService;
        private readonly INativeCalls nativeCalls;
        private bool isPristine = true;
        private string? email;
        private string? password;
        private string? firstName;
        private string? lastName;
        private string? confirmPassword;
        private bool? isAcceptingTheUserAgreement;

        public RegistrationViewModel(
            IIdentityService identityService,
            INavigationService navigationService,
            ILocalizationService localizationService,
            INativeCalls nativeCalls)
        {
            _identityService = identityService;
            _navigationService = navigationService;
            this.localizationService = localizationService;
            this.nativeCalls = nativeCalls;
            RegisterCommand = new Command(async () => await ExecuteRegisterCommand(), () => !IsPristine && Validate());
            ShowUserAgreementCommand = new Command(async () => await navigationService.PushModalAsync<UserAgreementViewModel>());
        }

        private async Task ExecuteRegisterCommand()
        {
            try
            {
                var model = new RegistrationModel()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                };

                await _identityService.RegisterAsync(model);

                await Authenticate(model);
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

        private async Task Authenticate(RegistrationModel registrationModel)
        {
            try
            {
                bool response = await _identityService.AuthenticateAsync(registrationModel.Email!, registrationModel.Password!);
                if (response)
                {
                    await _navigationService.PushAsync<AppShellViewModel>();
                }
                else
                {
                    nativeCalls.OpenToast(localizationService.GetString(nameof(AppResources.InvalidEmailOrPassword)));
                }
            }
            catch (HttpRequestException exc)
            {
                nativeCalls.OpenToast(exc.Message);
                await _navigationService.PopAsync();
            }
        }

        public Command RegisterCommand { get; }

        public Command ShowUserAgreementCommand { get; }


        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        [EmailAddress(ErrorMessageResourceName = nameof(AppResources.EmailIsInvalid), ErrorMessageResourceType = typeof(AppResources))]
        public string? Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = nameof(AppResources.FieldMinMaxLengthMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        [Compare(nameof(Password), ErrorMessageResourceName = nameof(AppResources.ConfirmPassword), ErrorMessageResourceType = typeof(AppResources))]
        public string? ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldMustAcceptUserAgreement), ErrorMessageResourceType = typeof(AppResources))]
        public bool? IsAcceptingUserAgreement
        {
            get => isAcceptingTheUserAgreement;
            set
            {
                if (!value.GetValueOrDefault())
                {
                    value = null;
                }
                SetProperty(ref isAcceptingTheUserAgreement, value);
            }
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
            ValidateProperty(value, propertyName);
            IsPristine = false;
            bool result = base.SetProperty(ref backingStore, value, propertyName, onChanged);
            RegisterCommand.ChangeCanExecute();
            return result;
        }
    }
}
