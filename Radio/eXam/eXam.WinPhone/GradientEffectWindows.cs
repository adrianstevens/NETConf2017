using eXam;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;

//add appropriate using statement for each windows platform variant

[assembly: ResolutionGroupName ("Xamarin")]
[assembly: ExportEffect (typeof (GradientEffectWindows), "GradientEffect")]
namespace eXam
{
	internal class GradientEffectWindows : PlatformEffect
	{
        Brush oldBrush;

		protected override void OnAttached()
		{
            var button = Element as Button;

            if (button == null)
                return;

            var gradBrush = new LinearGradientBrush()
            {
                StartPoint = new global::Windows.Foundation.Point(0, 0),
                EndPoint = new global::Windows.Foundation.Point(0, 1)
            };

            var color1 = Color.FromRgb(0, 0, 40);
            var color2 = button.BackgroundColor;

            gradBrush.GradientStops.Add(new GradientStop() { Color = GetWindowsColor(color1), Offset = 1 });
            gradBrush.GradientStops.Add(new GradientStop() { Color = GetWindowsColor(color2), Offset = 0 });

            var btn = Control as global::Windows.UI.Xaml.Controls.Button;

            oldBrush = btn.Background;
            btn.Background = gradBrush;
        }

		protected override void OnDetached()
		{
            var btn = Control as global::Windows.UI.Xaml.Controls.Button;

            if (btn == null)
                return;

            btn.Background = oldBrush;
        }

		protected override void OnElementPropertyChanged (System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged (args);
		}

        global::Windows.UI.Color GetWindowsColor (Color color)
        {
            return global::Windows.UI.Color.FromArgb((byte)(255 * color.A), (byte)(255 * color.R), (byte)(255 * color.G), (byte)(255 * color.B));
        }
	}
}