using System;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IIdentityService
    {
        bool IsAuthenticated { get; }

        Task<bool> AuthenticateAsync(string email, string password);
        Task RegisterAsync(RegistrationModel registrationModel);
        Task LogOutAsync();

        event EventHandler<EventArgs>? LoggedOut;
    }
}
