using System;
using Xamarin.Forms;

namespace XFDraw.Views
{
    public class ColorPickerMouse : ColorPicker
    {
        public ColorPickerMouse ()
        {
            Content = new Grid()
            {
                Padding = 6,
                BackgroundColor = Color.FromRgb(230, 230, 230),
            };

            AddButtons((Grid)Content);
        }

        void AddButtons(Grid grid)
        {
            int count = 80;
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
