using System.Threading.Tasks;

namespace App1.Services
{
    public interface IAlertService
    {
        Task DisplayAlertAsync(string title, string message, string cancel);
        Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = null, int maxLength = -1);
    }
}
