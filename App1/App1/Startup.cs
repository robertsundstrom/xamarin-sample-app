using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using App1.Helpers;
using App1.MobileAppService.Client;
using App1.Resources;
using App1.Services;
using App1.ViewModels;
using App1.Views;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Polly;

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

            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IIdentityService, IdentityService>();

            services.AddTransient<AppShellViewModel>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<ViewModels.RegistrationViewModel>();

            services.AddTransient<ItemsViewModel>();
            services.AddTransient<AboutViewModel>();

            services.AddSingleton<LoginPage>();
            services.AddSingleton<RegistrationPage>();

            services.AddSingleton<AppShell>();
            services.AddSingleton<App>();

            services.AddTransient<IResourceContainer, ResourceContainer>();
            services.AddTransient<ILocalizationService, LocalizationService>();

            if (App.UseMockDataStore)
            {
                services.AddTransient<IDataStore<Item>, MockDataStore>();
            }
            else
            {
                services.AddHttpClient<IRegistrationClient, RegistrationClient>(CreateClientDelegate)
                    .ConfigurePrimaryHttpMessageHandler(h => GetClientHandler())
                    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    }));

                services.AddHttpClient<ITokenClient, TokenClient>(CreateClientDelegate)
                    .ConfigurePrimaryHttpMessageHandler(h => GetClientHandler())
                    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    }));

                services.AddHttpClient<IItemsClient, ItemsClient>(CreateClientDelegate)
                    .ConfigurePrimaryHttpMessageHandler(h => GetClientHandler())
                    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    }));

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

        private static void CreateClientDelegate(IServiceProvider sp, HttpClient client)
        {
            client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sp.GetRequiredService<ISettingsService>().AuthAccessToken);
        }

        public static HttpClientHandler GetClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    //if (cert.Issuer.Equals("CN=localhost"))
                    //{
                    //    return true;
                    //}

                    //return errors == System.Net.Security.SslPolicyErrors.None;

                    return true;
                }
            };
            return handler;
        }
    }
}
