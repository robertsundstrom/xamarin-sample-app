using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using App1.Resources;
using App1.Services;

using AutoMapper;

using Xamarin.Forms;

namespace App1.ViewModels
{
    [System.Runtime.InteropServices.Guid("44B6C891-5DAD-4932-9E5A-7D2E2CE34F2A")]
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;
        private readonly INavigationService _navigationService;
        private readonly ILocalizationService localizationService;
        private readonly IAlertService alertService;
        private readonly IMapper mapper;
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
            IAlertService alertService,
            IMapper mapper)
        {
            _identityService = identityService;
            _navigationService = navigationService;

            this.localizationService = localizationService;
            this.alertService = alertService;
            this.mapper = mapper;

            RegisterCommand = new Command(async () => await ExecuteRegisterCommand(), () => !IsPristine && !IsBusy);
            ShowUserAgreementCommand = new Command(async () => await navigationService.PushModalAsync<UserAgreementViewModel>(), () => !IsBusy);
        }

        private async Task ExecuteRegisterCommand()
        {
            if (!Validate())
            {
                await alertService.DisplayAlertOkAsync(string.Empty, localizationService.GetString(nameof(AppResources.CheckFieldsMessage)));
                IsBusy = false;
                return;
            }

            IsBusy = true;

            try
            {
                var model = mapper.Map<RegistrationViewModel, RegistrationModel>(this);

                await _identityService.RegisterAsync(model);

                await Authenticate(model);
            }
            catch (HttpRequestException exc)
            {
                await alertService.DisplayAlertOkAsync(string.Empty, exc.Message);
            }
            catch (Exception exc)
            {
                await alertService.DisplayAlertOkAsync(string.Empty, exc.Message);
            }
            finally
            {
                IsBusy = false;
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
                    await alertService.DisplayAlertOkAsync(string.Empty, localizationService.GetString(nameof(AppResources.InvalidEmailOrPassword)));
                }
            }
            catch (HttpRequestException exc)
            {
                await alertService.DisplayAlertOkAsync(string.Empty, exc.Message);
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

        [Required(ErrorMessageResourceName = nameof(AppResources.ConfirmPasswordMessage), ErrorMessageResourceType = typeof(AppResources))]
        [DataAnnotations.Compare(nameof(Password), ErrorMessageResourceName = nameof(AppResources.PasswordsAreNotEqual), ErrorMessageResourceType = typeof(AppResources))]
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
            if (ValidateProperty(value, false, propertyName))
            {
                RemoveErrors(propertyName);
            }
            IsPristine = false;
            bool result = base.SetProperty(ref backingStore, value, propertyName, onChanged);
            RegisterCommand.ChangeCanExecute();
            return result;
        }
    }
}
