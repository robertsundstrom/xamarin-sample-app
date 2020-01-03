using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using App1.MobileAppService.Client;
using App1.Resources;
using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    class UpdateUserProfileViewModel : ViewModelBase<User>
    {
        private readonly IUserClient userClient;
        private readonly INavigationService navigationService;
        private readonly ILocalizationService localizationService;
        private readonly INativeCalls nativeCalls;
        private User user;
        private string? firstName;
        private string? lastName;
        private string? email;
        private bool isPristine;

        public UpdateUserProfileViewModel(
            IUserClient userClient,
            INavigationService navigationService,
            ILocalizationService localizationService,
            INativeCalls nativeCalls)
        {
            this.userClient = userClient;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.nativeCalls = nativeCalls;

            UpdateUserProfileCommand = new Command(async () => await ExecuteUpdateUserProfileCommand(), () => !IsPristine);
        }

        public override Task InitializeAsync(User arg)
        {
            FirstName = arg.FirstName;
            LastName = arg.LastName;
            Email = arg.Email;

            return Task.CompletedTask;
        }

        private async Task ExecuteUpdateUserProfileCommand()
        {
            if (!Validate())
            {
                await nativeCalls.OpenToast(string.Empty, localizationService.GetString(nameof(AppResources.CheckFieldsMessage)));
                return;
            }

            try
            {
                var model = new UpdateUser()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                };

                await userClient.UpdateUserAsync(model);

                await nativeCalls.OpenToast(string.Empty, localizationService.GetString(nameof(AppResources.UserProfileUpdatedMessage)));

                await navigationService.PopAsync();

            }
            catch (HttpRequestException exc)
            {
                await nativeCalls.OpenToast(string.Empty, exc.Message);
            }
            catch (Exception exc)
            {
                await nativeCalls.OpenToast(string.Empty, exc.Message);
            }
        }

        public Command UpdateUserProfileCommand { get; }

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
            UpdateUserProfileCommand.ChangeCanExecute();
            return result;
        }
    }
}
