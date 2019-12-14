using System;

using Microsoft.Extensions.DependencyInjection;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "https://192.168.1.83:5001" : "https://192.168.1.83:5001";
        public static bool UseMockDataStore = false;

        public static IServiceProvider ServiceProvider { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = ServiceProvider.GetService<AppShell>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
