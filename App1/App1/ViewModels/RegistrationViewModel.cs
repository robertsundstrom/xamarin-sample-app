using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;
        private readonly INavigationService _navigationService;
        private readonly INativeCalls nativeCalls;
        private bool isClean = true;
        private string? email;
        private string? password;
        private string? firstName;
        private string? lastName;

        public RegistrationViewModel(IIdentityService identityService, INavigationService navigationService, INativeCalls nativeCalls)
        {
            _identityService = identityService;
            _navigationService = navigationService;
            this.nativeCalls = nativeCalls;
            RegisterCommand = new Command(async () => await ExecuteRegisterCommand(), () => CanSubmit);
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
                var response = await _identityService.AuthenticateAsync(registrationModel.Email!, registrationModel.Password!);
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
                await _navigationService.PopAsync();
            }
        }

        public Command RegisterCommand { get; }


        [Required]
        public string? FirstName
        {
            get => firstName;
            set
            {
                ValidateProperty(value);
                isClean = false;
                SetProperty(ref firstName, value);
                RegisterCommand.ChangeCanExecute();
            }
        }

        [Required]
        public string? LastName
        {
            get => lastName;
            set
            {
                ValidateProperty(value);
                isClean = false;
                SetProperty(ref lastName, value);
                RegisterCommand.ChangeCanExecute();
            }
        }

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
                RegisterCommand.ChangeCanExecute();
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
                RegisterCommand.ChangeCanExecute();
            }
        }

        protected override void ValidateProperty<T>(T value, [CallerMemberName] string? propertyName = null)
        {
            base.ValidateProperty(value, propertyName);

            OnPropertyChanged(nameof(CanSubmit));
        }

        public bool CanSubmit => !isClean && Validate();
    }
}
