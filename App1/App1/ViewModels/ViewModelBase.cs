using System.Threading.Tasks;

namespace App1.ViewModels
{
    public abstract class ViewModelBase : ValidationBase, IViewModelBase
    {
        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string? title = string.Empty;
        public string? Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public virtual Task InitializeAsync(object? arg)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class ViewModelBase<TArg> : ViewModelBase, IViewModelBase<TArg>
    {
        public override Task InitializeAsync(object? arg)
        {
            return ((IViewModelBase<TArg>)this).InitializeAsync((TArg)arg!);
        }

        public virtual Task InitializeAsync(TArg arg)
        {
            return Task.CompletedTask;
        }
    }

    internal interface IViewModelBase
    {
        Task InitializeAsync(object? arg);
    }

    internal interface IViewModelBase<TArg>
    {
        Task InitializeAsync(TArg arg);
    }
}
