using System;

using Microsoft.Extensions.DependencyInjection;

namespace App1.ViewModels
{
    internal static class ViewModelLocator
    {
        public static IServiceProvider? ServiceProvider { get; internal set; }

        public static ItemsViewModel Items => ServiceProvider.GetRequiredService<ItemsViewModel>();

        public static AboutViewModel About => ServiceProvider.GetRequiredService<AboutViewModel>();

        public static UserProfileViewModel UserProfile => ServiceProvider.GetRequiredService<UserProfileViewModel>();
    }
}
