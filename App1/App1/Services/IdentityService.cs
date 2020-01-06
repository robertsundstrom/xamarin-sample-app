using System;
using System.Security;
using System.Threading.Tasks;

using App1.MobileAppService.Client;

namespace App1.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ISettingsService settingService;
        private readonly ITokenClient tokenClient;
        private readonly IRegistrationClient registrationClient;

        public event EventHandler<EventArgs>? LoggedOut;

        public IdentityService(ISettingsService settingService, ITokenClient tokenClient, IRegistrationClient registrationClient)
        {
            this.settingService = settingService;
            this.tokenClient = tokenClient;
            this.registrationClient = registrationClient;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            try
            {
                var result = await tokenClient.AuthenticateAsync(email, password);
                settingService.AuthAccessToken = result.Token;

                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        public async Task RegisterAsync(RegistrationModel registrationModel)
        {
            await registrationClient.RegisterAsync(new Registration()
            {
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                Email = registrationModel.Email,
                Password = registrationModel.Password
            });
        }

        public Task LogOutAsync()
        {
            return Task.Run(() =>
            {
                settingService.AuthAccessToken = null;
                LoggedOut?.Invoke(this, EventArgs.Empty);
            });
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(settingService.AuthAccessToken);
    }

    public class RegistrationModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
