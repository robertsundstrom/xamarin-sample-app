using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using App1.MobileAppService.Configuration;
using App1.MobileAppService.Data;
using App1.MobileAppService.Hubs;
using App1.MobileAppService.Models;
using App1.MobileAppService.Services;
using App1.Models;

using AutoMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using NJsonSchema;

namespace App1.MobileAppService
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson();

            services.AddDbContext<ApplicationDbContext>
              (options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")))
                  .AddIdentityCore<User>(options =>
                  {
                      options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
                  })
                 .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });

            var jwtConfig = Configuration.GetSection("Jwt").Get<JwtConfiguration>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            //if (!string.IsNullOrEmpty(accessToken) &&
                            //    (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                            //{
                            //    context.Token = context.Request.Query["access_token"];
                            //}
                            // If the request is for our hub...

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSignalR();

            services.AddSwaggerDocument(x =>
            {
                x.SchemaType = SchemaType.OpenApi3;
                x.SchemaNameGenerator = new CustomSchemaNameGenerator();
            });
            services.AddScoped<IItemRepository, ItemRepository>();

            services.AddTransient<IJwtTokenService, JwtTokenService>();

            services.AddAutoMapper(typeof(Startup).Assembly);
            
            services.AddSingleton<IProfileImageUploader>(sp =>
            {
                var azureConfig = Configuration.GetSection("Azure").Get<AzureConfiguration>();
                string storageConnectionString = azureConfig?.BlobStorage?.ConnectionString;
                CloudStorageAccount storageAccount;
                if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
                {
                    return new ProfileImageUploader(storageAccount.CreateCloudBlobClient());
                }

                throw new Exception();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ItemsHub>("/itemsHub");
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
