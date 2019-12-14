using System;

using Android.Support.V4.Content;

using App1.Droid.Effects;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("App1.Effects")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]

namespace App1.Droid.Effects
{
    public class BorderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var drawable = ContextCompat.GetDrawable(Control.Context, Resource.Drawable.border);
                Control.SetBackground(drawable);
            }
            catch (Exception)
            {
            }
        }

        protected override void OnDetached()
        {
            try
            {
                Control.SetBackground(null);
            }
            catch (Exception)
            {
            }
        }
    }
}
