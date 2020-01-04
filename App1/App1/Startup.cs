using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using App1.Configuration;
using App1.Helpers;
using App1.MobileAppService.Client;
using App1.Resources;
using App1.Services;
using App1.ViewModels;
using App1.Views;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Polly;

using Xamarin.Essentials;

using ChangePasswordViewModel = App1.ViewModels.ChangePasswordViewModel;

namespace App1
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            string systemDir = FileSystem.CacheDirectory;

            Utils.ExtractSaveResource("App1.appsettings.json", systemDir);
            Utils.ExtractSaveResource("App1.appsettings.Development.json", systemDir);

            string fullConfig = Path.Combine(systemDir, "App1.appsettings.json");
            string fullDevConfig = Path.Combine(systemDir, "App1.appsettings.Development.json");


            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddJsonFile(fullConfig);
                                c.AddJsonFile(fullDevConfig, optional: true);
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
                string world = ctx.Configuration["Hello"];
            }

            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IIdentityService, IdentityService>();

            services.AddTransient<AppShellViewModel>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<ViewModels.RegistrationViewModel>();

            services.AddTransient<ItemsViewModel>();
            services.AddTransient<UserAgreementViewModel>();
            services.AddTransient<UserProfileViewModel>();
            services.AddTransient<EditUserProfileViewModel>();
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<AboutViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<UserProfilePage>();
            services.AddTransient<EditUserProfilePage>();
            services.AddTransient<ChangePasswordPage>();
            services.AddTransient<RegistrationPage>();
            services.AddTransient<UserAgreementPage>();
            services.AddTransient<AboutPage>();

            services.AddTransient<AppShell>();
            services.AddSingleton<App>();

            services.AddSingleton<IAlertService, AlertService>();

            services.AddTransient<IResourceContainer, ResourceContainer>();
            services.AddTransient<ILocalizationService, LocalizationService>();


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


            services.AddSingleton<IItemsHubClient, ItemsHubClient>(sp =>
            {
                var conf = sp.GetService<IConfiguration>();
                var appServiceConfig = conf.GetSection("MobileAppService").Get<AppServiceConfiguration>();

                var hubConnection = new HubConnectionBuilder().WithUrl($"{appServiceConfig.ServiceEndpoint}/itemsHub", opt =>
                {
                    //opt.Transports = HttpTransportType.WebSockets;
                    opt.AccessTokenProvider = () => Task.FromResult(sp.GetRequiredService<ISettingsService>().AuthAccessToken);
                })
                        .WithAutomaticReconnect()
                        .Build();

                hubConnection.StartAsync();

                return new ItemsHubClient(hubConnection);
            });

            services.AddHttpClient<IUserClient, UserClient>(CreateClientDelegate)
                .ConfigurePrimaryHttpMessageHandler(h => GetClientHandler())
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                }));

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
            var conf = sp.GetService<IConfiguration>();
            var appServiceConfig = conf.GetSection("MobileAppService").Get<AppServiceConfiguration>();

            client.BaseAddress = new Uri($"{appServiceConfig.ServiceEndpoint}/");
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
