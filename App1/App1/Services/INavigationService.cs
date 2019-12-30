using System.Threading.Tasks;

using App1.ViewModels;

namespace App1.Services
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task PopAsync();
        Task PopModalAsync();
        Task PopToRootAsync();
        Task PushAsync<TViewModel>(object? arg = null) where TViewModel : ViewModelBase;
        Task PushModalAsync<TViewModel>(object? arg = null) where TViewModel : ViewModelBase;
    }
}
