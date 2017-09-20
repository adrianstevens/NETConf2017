using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFDraw.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerMouseView : ColorPicker
	{
		public ColorPickerMouseView ()
		{
			InitializeComponent ();

            AddButtons((Grid)Content);
        }

        void AddButtons(Grid grid)
        {
            int count = 100;
            int columns = 20;
            int size = 16;

            for (int i = 0; i < count; i++)
            {
                double hue = (i % columns) * 1.0 / columns;
                double saturation = 0.9;
                double luminocity = 0.4 + i / columns / 10.0;


                var button = GetColorButton(Color.FromHsla(hue, saturation, luminocity), size);
                Grid.SetColumn(button, i % columns);
                Grid.SetRow(button, i / columns);

                button.ColorButtonTapped += ButtonColorButtonTapped;
                grid.Children.Add(button);
            }
        }
	}
}