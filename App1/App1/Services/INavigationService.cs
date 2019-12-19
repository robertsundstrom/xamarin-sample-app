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
        Task PushAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task PushAsync<TViewModel, TArg>(TArg arg) where TViewModel : ViewModelBase<TArg>;
        Task PushModalAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task PushModalAsync<TViewModel, TArg>(TArg arg) where TViewModel : ViewModelBase<TArg>;
    }
}
