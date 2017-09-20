using XFDraw;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XFDraw.UWP;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(SketchView), typeof(SketchViewRenderer))]
namespace XFDraw.UWP
{
    class SketchViewRenderer : ViewRenderer<SketchView, PaintView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SketchView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var paintView = new PaintView();
                paintView.SetInkColor(GetWindowsColor(Element.InkColor));
                paintView.LineDrawn += PaintViewLineDrawn;
                SetNativeControl(paintView);

                MessagingCenter.Subscribe<SketchView>(this, "Clear", OnMessageClear);
            }
        }

        void OnMessageClear(SketchView sender)
        {
            if (sender == Element)
                Control.Clear();
        }

        private void PaintViewLineDrawn(object sender, System.EventArgs e)
        {
            ((ISketchController)Element)?.SendSketchUpdated();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName == SketchView.InkColorProperty.PropertyName)
            {
                Control.SetInkColor(GetWindowsColor(Element.InkColor));
            }
        }

        Windows.UI.Color GetWindowsColor(Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(255 * color.A), (byte)(255 * color.R), (byte)(255 * color.G), (byte)(255 * color.B));
        }

        protected override void Dispose(bool disposing)
        {
            MessagingCenter.Unsubscribe<SketchView>(this, "Clear");
            Control.LineDrawn -= PaintViewLineDrawn;
        }
    }
}