using System;
using Xamarin.Forms;

namespace XFDraw.Views
{
    public abstract class ColorPicker : ContentView
    {
        public event EventHandler ColorChanged;

        public Color CurrentColor { get; private set; }

        protected ColorButton buttonSelected;

        public ColorPicker()
        {
        }

        protected void ButtonColorButtonTapped(object sender, EventArgs e)
        {
            CurrentColor = ((ColorButton)sender).Color;
            ColorChanged?.Invoke(this, EventArgs.Empty);

            if (buttonSelected != null)
                buttonSelected.ScaleTo(scale: 1.0, length: 150);

            buttonSelected = (ColorButton)sender;
            buttonSelected.ScaleTo(scale: 1.15, length: 150);
        }

        protected ColorButton GetColorButton(Color color, int size)
        {
            return new ColorButton()
            {
                WidthRequest = size,
                HeightRequest = size,
                Color = color
            };
        }
    }
}