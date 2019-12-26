using System;
using System.Collections.Generic;

using App1.Styles;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(App1.iOS.Renderers.PageRenderer))]
namespace App1.iOS.Renderers
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public new ContentPage Element => (ContentPage)base.Element;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController == null)
            {
                return;
            }

            var leftNavList = new List<UIBarButtonItem>();
            var rightNavList = new List<UIBarButtonItem>();

            var navigationItem = NavigationController.TopViewController.NavigationItem;

            // For hamburger item
            if (navigationItem.LeftBarButtonItems?.Length > 0)
            {
                var LeftNavItems = navigationItem.LeftBarButtonItems[0];
                leftNavList.Add(LeftNavItems);
            }

            for (int i = 0; i < Element.ToolbarItems.Count; i++)
            {
                int reorder = (Element.ToolbarItems.Count - 1);
                int ItemPriority = Element.ToolbarItems[reorder - i].Priority;

                if (ItemPriority == 1)
                {
                    var LeftNavItems = navigationItem.RightBarButtonItems[i];
                    leftNavList.Add(LeftNavItems);
                }
                else if (ItemPriority == 0)
                {
                    var RightNavItems = navigationItem.RightBarButtonItems[i];
                    rightNavList.Add(RightNavItems);
                }
            }

            navigationItem.SetLeftBarButtonItems(leftNavList.ToArray(), false);
            navigationItem.SetRightBarButtonItems(rightNavList.ToArray(), false);
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetTheme();
            }
            catch (Exception) { }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);

            if (TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetTheme();
            }
        }

        private void SetTheme()
        {
            if (TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                App.Current.Resources = new DarkTheme();
            }
            else
            {
                App.Current.Resources = new LightTheme();
            }
        }
    }
}
