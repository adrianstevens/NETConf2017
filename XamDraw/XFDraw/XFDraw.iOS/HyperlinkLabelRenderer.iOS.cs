using Foundation;
using XFDraw;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFDraw.iOS;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace XFDraw.iOS
{
    class HyperlinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control.UserInteractionEnabled = true;

            Control.TextColor = UIColor.Blue;
                
            var gesture = new UITapGestureRecognizer();

            gesture.AddTarget(() =>
            {
                var url = new NSUrl("https://" + Control.Text);

                if (UIApplication.SharedApplication.CanOpenUrl(url))
                    UIApplication.SharedApplication.OpenUrl(url);
            });

            Control.AddGestureRecognizer(gesture);
        }
    }
}