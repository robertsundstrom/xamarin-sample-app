﻿using Microsoft.Extensions.DependencyInjection;

using System;

namespace App1.ViewModels
{
    internal static class ViewModelLocator
    {
        public static IServiceProvider ServiceProvider { get; internal set; }

        public static ItemsViewModel Items => ServiceProvider.GetRequiredService<ItemsViewModel>();

        public static AboutViewModel About => ServiceProvider.GetRequiredService<AboutViewModel>();
    }
}