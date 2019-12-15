using System;
using System.IO;

using App1.Helpers;
using App1.MobileAppService.Client;
using App1.Services;
using App1.ViewModels;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xamarin.Essentials;

namespace App1
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var systemDir = FileSystem.CacheDirectory;
            Utils.ExtractSaveResource("App1.appsettings.json", systemDir);
            var fullConfig = Path.Combine(systemDir, "App1.appsettings.json");

            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddJsonFile(fullConfig);
                            })
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();

            App.ServiceProvider = host.Services;
            ViewModelLocator.ServiceProvider = host.Services;

            return App.ServiceProvider.GetService<App>();
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                var world = ctx.Configuration["Hello"];
            }

            services.AddTransient<ItemsViewModel>();
            services.AddTransient<AboutViewModel>();

            services.AddTransient<AppShell>();
            services.AddSingleton<App>();

            services.AddTransient<ILocalizationService, LocalizationService>();

            if (App.UseMockDataStore)
            {
                services.AddTransient<IDataStore<Item>, MockDataStore>();
            }
            else
            {
                services.AddHttpClient<IItemsClient, ItemsClient>(client =>
                {
                    client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");
                });

                services.AddSingleton<IDataStore<Item>, AzureDataStore>();
            }

#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (certificate.Issuer.Equals("CN=localhost"))
                {
                    return true;
                }

                return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
            };
#endif
        }

    }
}
