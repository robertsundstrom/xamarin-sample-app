using System.Threading.Tasks;

namespace App1.Services
{
    public interface IAlertService
    {
        Task DisplayAlertAsync(string title, string message, string cancel);
    }
}
