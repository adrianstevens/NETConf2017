using Android.Text.Util;
using Android.Widget;
using XFDraw;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFDraw.Droid;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace XFDraw.Droid
{
    class HyperlinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Linkify.AddLinks(Control, MatchOptions.All);
        }
    }
}
