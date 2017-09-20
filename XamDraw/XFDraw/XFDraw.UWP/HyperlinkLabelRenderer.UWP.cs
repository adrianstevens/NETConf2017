using XFDraw;
using XFDraw.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.System;
using System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace XFDraw.UWP
{
    class HyperlinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control.Foreground = new SolidColorBrush(Colors.Blue);
     
            Control.Tapped += LabelTapped;
        }

        private async void LabelTapped(object sender, TappedRoutedEventArgs e)
        {
            var website = Element.Text;

            if (website.IndexOf("http://") == -1)
                website = "http://" + website;

            await Launcher.LaunchUriAsync(new Uri(website));
        }
    }
}
