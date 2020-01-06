using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using App1.MobileAppService.Client;
using App1.Resources;
using App1.Services;

using AutoMapper;

using Xamarin.Forms;

namespace App1.ViewModels
{
    class EditUserProfileViewModel : ViewModelBase<User>
    {
        private readonly IUserClient userClient;
        private readonly INavigationService navigationService;
        private readonly ILocalizationService localizationService;
        private readonly IAlertService alertService;
        private readonly IMapper mapper;
        private string? firstName;
        private string? middleName;
        private string? lastName;
        private string? email;
        private bool isPristine;

        public EditUserProfileViewModel(
            IUserClient userClient,
            INavigationService navigationService,
            ILocalizationService localizationService,
            IAlertService alertService,
            IMapper mapper)
        {
            this.userClient = userClient;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.alertService = alertService;
            this.mapper = mapper;
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
                await alertService.DisplayAlertAsync(string.Empty, localizationService.GetString(nameof(AppResources.CheckFieldsMessage)), "OK");
                return;
            }

            try
            {
                var model = mapper.Map<EditUserProfileViewModel, UpdateUser>(this);

                await userClient.UpdateUserAsync(model);

                await alertService.DisplayAlertAsync(string.Empty, localizationService.GetString(nameof(AppResources.UserProfileUpdatedMessage)), "OK");

                await navigationService.PopAsync();

            }
            catch (HttpRequestException exc)
            {
                await alertService.DisplayAlertAsync(string.Empty, exc.Message, "OK");
            }
            catch (Exception exc)
            {
                await alertService.DisplayAlertAsync(string.Empty, exc.Message, "OK");
            }
        }

        public Command UpdateUserProfileCommand { get; }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string? MiddleName
        {
            get => middleName;
            set => SetProperty(ref middleName, value);
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
