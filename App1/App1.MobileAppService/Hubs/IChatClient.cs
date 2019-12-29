using System.Threading.Tasks;

namespace App1.MobileAppService.Hubs
{
    public interface IChatClient
    {
        Task OnMessageReceived(Models.Dtos.Message message);
    }
}
