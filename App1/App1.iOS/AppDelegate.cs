﻿
using System.Reflection;
using System.Resources;

using App1.Resources;

using Foundation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using UIKit;

namespace App1.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(Startup.Init(ConfigureServices));

            return base.FinishedLaunching(app, options);
        }

        private void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddSingleton<INativeCalls, NativeCalls>();

            services.AddSingleton<IResourceContainer>(sp => new ResourceContainer(new ResourceManager(ResourceContainer.ResourceId, typeof(AppResource).GetTypeInfo().Assembly), new Localize()));
        }
    }
}
