using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    class ChangePasswordViewModel : ViewModelBase<User>
    {
        private readonly IUserClient userClient;
        private readonly INavigationService navigationService;
        private readonly ILocalizationService localizationService;
        private readonly IAlertService alertService;
        private readonly IMapper mapper;
        private string? confirmNewPassword;
        private bool isPristine;
        private string? currentPassword;
        private string? newPassword;

        public ChangePasswordViewModel(
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

            UpdatePasswordCommand = new Command(async () => await ExecuteUpdatePasswordCommand(), () => !IsPristine);
        }

        private async Task ExecuteUpdatePasswordCommand()
        {
            if (!Validate())
            {
                await alertService.DisplayAlertAsync(string.Empty, localizationService.GetString(nameof(AppResources.CheckFieldsMessage)), "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var model = mapper.Map<ChangePasswordViewModel, ChangePasswordRequest>(this);

                await userClient.ChangePasswordAsync(model);

                await alertService.DisplayAlertAsync(string.Empty, localizationService.GetString(nameof(AppResources.UserProfileUpdatedMessage)), "OK");

                await navigationService.PopAsync();

            }
            catch (ApiException<ProblemDetails>)
            {
                await alertService.DisplayAlertAsync(string.Empty, "Invalid password.", "OK");

            }
            catch (ApiException<ICollection<IdentityError>> exc)
            {
                await alertService.DisplayAlertAsync(string.Empty, string.Join(",", exc.Result.Select(x => x.Description)), "OK");
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

        public Command UpdatePasswordCommand { get; }

        [Required(ErrorMessageResourceName = nameof(AppResources.ConfirmPasswordMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? CurrentPassword
        {
            get => currentPassword;
            set => SetProperty(ref currentPassword, value);
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = nameof(AppResources.FieldMinMaxLengthMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.ConfirmPasswordMessage), ErrorMessageResourceType = typeof(AppResources))]
        [DataAnnotations.Compare(nameof(NewPassword), ErrorMessageResourceName = nameof(AppResources.PasswordsAreNotEqual), ErrorMessageResourceType = typeof(AppResources))]
        public string? ConfirmNewPassword
        {
            get => confirmNewPassword;
            set => SetProperty(ref confirmNewPassword, value);
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
            UpdatePasswordCommand.ChangeCanExecute();
            return result;
        }
    }
}
