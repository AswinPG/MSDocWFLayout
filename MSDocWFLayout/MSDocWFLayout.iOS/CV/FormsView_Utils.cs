using UIKit;
using Xamarin.Forms;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;

namespace MSDocWFLayout.iOS.CV
{
    internal static class FormsView_Utils
    {
        public static UIView ToUIView(this View view, CGRect size)
        {
            var renderer = Platform.CreateRenderer(view);
            //EventTracker eventTracker = new EventTracker(renderer);

            renderer.NativeView.Frame = size;

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            renderer.Element.Layout(size.ToRectangle());

            var nativeView = renderer.NativeView;
            nativeView.SetNeedsLayout();

            return nativeView;
        }
    }
}