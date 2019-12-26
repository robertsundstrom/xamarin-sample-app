using System;
using System.Threading.Tasks;

using App1.Services;

using Xamarin.Forms;

namespace App1
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = false;

        public static IServiceProvider? ServiceProvider { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new ContentPage();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await InitNavigation();

            base.OnResume();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private Task InitNavigation()
        {
            var navigationService = (INavigationService)ServiceProvider!.GetService(typeof(INavigationService));
            return navigationService.InitializeAsync();
        }
    }
}
