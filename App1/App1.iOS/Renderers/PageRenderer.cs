using System.Collections.Generic;

using UIKit;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ContentPage), typeof(App1.iOS.Renderers.PageRenderer))]
namespace App1.iOS.Renderers
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public new ContentPage Element => (ContentPage)base.Element;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var leftNavList = new List<UIBarButtonItem>();
            var rightNavList = new List<UIBarButtonItem>();

            var navigationItem = NavigationController.TopViewController.NavigationItem;

            for (var i = 0; i < Element.ToolbarItems.Count; i++)
            {

                var reorder = (Element.ToolbarItems.Count - 1);
                var ItemPriority = Element.ToolbarItems[reorder - i].Priority;

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
    }
}
