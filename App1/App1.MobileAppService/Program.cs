using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App1.MobileAppService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((context, configBuilder) =>
                 {
                     ConfigureAzureKeyVault(context, configBuilder);
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        private static void ConfigureAzureKeyVault(HostBuilderContext context, IConfigurationBuilder configBuilder)
        {
            var builder = configBuilder.Build();

            var keyVaultEndpoint = builder["AzureKeyVaultEndpoint"];

            if (context.HostingEnvironment.IsProduction() && keyVaultEndpoint != null)
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();

                var keyVaultClient = new KeyVaultClient(
                  new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback)
                  );

                configBuilder.AddAzureKeyVault(keyVaultEndpoint);
            }
        }
    }
}
