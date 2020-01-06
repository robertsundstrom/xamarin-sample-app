using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using App1.Configuration;
using App1.Data;
using App1.Helpers;
using App1.MobileAppService.Client;
using App1.Resources;
using App1.Services;
using App1.ViewModels;
using App1.Views;

using AutoMapper;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Polly;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            string systemDir = FileSystem.CacheDirectory;

            const string AppSettingsFileName = "App1.appsettings.json";
            const string AppSettingsDevFileName = "App1.appsettings.Development.json";

            Utils.ExtractSaveResource(AppSettingsFileName, systemDir);
            Utils.ExtractSaveResource(AppSettingsDevFileName, systemDir);

            string fullConfig = Path.Combine(systemDir, AppSettingsFileName);
            string fullDevConfig = Path.Combine(systemDir, AppSettingsDevFileName);

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

            services.AddSingleton<App>();

            RegisterViewModels(services);

            RegisterPages(services);

            AddServices(services);

            AddHttpClients(services);

            AddSignaĺRClients(services);

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

        private const string databaseName = "database.db";

        private static void AddServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                String databasePath = "";
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        SQLitePCL.Batteries_V2.Init();
                        databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                        break;
                    case Device.Android:
                        databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                        break;
                    default:
                        throw new NotImplementedException("Platform not supported");
                }
                optionsBuilder.UseSqlite($"Filename={databasePath}");
            });

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddSingleton<IAlertService, AlertService>();

            services.AddTransient<IResourceContainer, ResourceContainer>();
            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IIdentityService, IdentityService>();

            services.AddSingleton<IDataStore<Models.Item>, DataStore>();
        }

        private static void AddSignaĺRClients(IServiceCollection services)
        {
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
        }

        private static void AddHttpClients(IServiceCollection services)
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

            services.AddHttpClient<IUserClient, UserClient>(CreateClientDelegate)
                .ConfigurePrimaryHttpMessageHandler(h => GetClientHandler())
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                }));
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            foreach (var viewModel in typeof(Startup)
                .Assembly
                .GetTypes()
                .Except(new[] { typeof(ViewModelBase), typeof(ViewModelBase<>) })
                .Where(t => typeof(ViewModelBase).IsAssignableFrom(t)))
            {
                services.AddTransient(viewModel);
            }
        }

        private static void RegisterPages(IServiceCollection services)
        {
            foreach (var viewModel in typeof(Startup)
                .Assembly
                .GetTypes()
                .Except(new[] { typeof(CustomNavigationView) })
                .Where(t => typeof(Page).IsAssignableFrom(t)))
            {
                services.AddTransient(viewModel);
            }
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
