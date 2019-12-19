﻿
using System;
using System.Reflection;
using System.Threading.Tasks;

using App1.ViewModels;
using App1.Views;

using Microsoft.Extensions.DependencyInjection;

using Xamarin.Forms;

namespace App1.Services
{
    public class NavigationService : INavigationService
    {
        private readonly App app;
        private readonly ISettingsService settingsService;
        private readonly IServiceProvider serviceProvider;

        public NavigationService(App app, ISettingsService settingsService, IServiceProvider serviceProvider)
        {
            this.app = app;
            this.settingsService = settingsService;
            this.serviceProvider = serviceProvider;
        }

        public Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(settingsService.AuthAccessToken))
            {
                return PushAsync<LoginViewModel>();
            }
            else
            {
                return PushAsync<AppShellViewModel>();
            }
        }

        public async Task PopAsync()
        {
            await app.MainPage.Navigation.PopAsync();
        }

        public async Task PopModalAsync()
        {
            await app.MainPage.Navigation.PopAsync();
        }

        public async Task PopToRootAsync()
        {
            await app.MainPage.Navigation.PopToRootAsync();
        }

        public async Task PushAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            await PushCoreAsync<TViewModel>(null);
        }

        public Task PushAsync<TViewModel, TArg>(TArg arg) where TViewModel : ViewModelBase<TArg>
        {
            return PushCoreAsync<TViewModel>(arg);
        }

        public async Task PushModalAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            var page = await InitializePageAsync<TViewModel>(null);
            await app.MainPage.Navigation.PushModalAsync(page);
        }

        public async Task PushModalAsync<TViewModel, TArg>(TArg arg) where TViewModel : ViewModelBase<TArg>
        {
            var page = await InitializePageAsync<TViewModel>(arg);
            await app.MainPage.Navigation.PushModalAsync(page);
        }

        private async Task PushCoreAsync<TViewModel>(object? arg) where TViewModel : ViewModelBase
        {
            var page = await InitializePageAsync<TViewModel>(arg);

            if (page is AppShell)
            {
                app.MainPage = page;

            }
            else
            {
                if (page is LoginPage)
                {
                    app.MainPage = new CustomNavigationView(page);

                }
                else
                {
                    await app.MainPage.Navigation.PushAsync(page);
                }
            }
        }

        private async Task<Page> InitializePageAsync<TViewModel>(object? arg) where TViewModel : ViewModelBase
        {
            var viewModelType = typeof(TViewModel);
            var pageType = typeof(TViewModel) == typeof(AppShellViewModel) ? typeof(AppShell) : GetPageTypeForViewModel(viewModelType);
            var page = (Page)serviceProvider.GetRequiredService(pageType);
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();

            page.BindingContext = viewModel;
            await viewModel.InitializeAsync(arg);

            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.Name.Replace("ViewModel", "Page");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = $"App1.Views.{viewName}, {viewModelAssemblyName}";
            return Type.GetType(viewAssemblyName);
        }
    }
}
